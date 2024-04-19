using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Sprinti.Button;
using Sprinti.Confirmation;
using Sprinti.Display;
using Sprinti.Instruction;
using Sprinti.Serial;
using Sprinti.Stream;
using Sprinti.Workflow;

namespace Sprinti;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureBuilder(builder);

        var app = builder.Build();

        Configure(app);

        // socat -d -d pty,raw,echo=0 pty,raw,echo=0
        app.Run();
    }

    private static void Configure(WebApplication app)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                swagger.Servers = new List<OpenApiServer>
                {
                    new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" },
                    new() { Url = "http://sprinti.secure.straessle.me:5000" }
                };
            });
        });

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sprinti");
            c.RoutePrefix = "";
        });

        var imagePath = app.Services.GetRequiredService<IOptions<StreamOptions>>().Value.Capture
            .ImagePathFromContentRoot;
        var imageFileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, imagePath));
        PathString imageRequestPath = $"/{imagePath}";
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = imageFileProvider,
            RequestPath = imageRequestPath
        });
        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = imageFileProvider,
            RequestPath = imageRequestPath
        });

        app.UseRouting();
        app.MapControllers();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("sprinti.json", optional: true, reloadOnChange: true);
        builder.Services.AddDirectoryBrowser();

        var serialOptions = builder.Configuration.GetSection(SerialOptions.Serial);
        builder.Services.Configure<SerialOptions>(serialOptions);
        var serialOptionsValue = serialOptions.Get<SerialOptions>();
        if (serialOptionsValue is { Enabled: true }) builder.Services.AddSerialModule();

        var streamOptions = builder.Configuration.GetSection(StreamOptions.Stream);
        builder.Services.Configure<StreamOptions>(streamOptions);
        var streamOptionsValue = streamOptions.Get<StreamOptions>();
        if (streamOptionsValue is { Enabled: true }) builder.Services.AddStreamModule(streamOptionsValue);

        builder.Services.AddInstructionModule();

        builder.Services.Configure<ConfirmationOptions>(
            builder.Configuration.GetSection(ConfirmationOptions.Confirmation));
        builder.Services.AddConfirmationModule();

        builder.Services.Configure<ButtonOptions>(
            builder.Configuration.GetSection(ButtonOptions.Button));
        builder.Services.AddButtonModule();

        builder.Services.AddDisplayModule();
        // builder.Services.AddWorkflowModule();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
        builder.Services.AddSwaggerGen(c =>
        {
            c.CustomOperationIds(e =>
                $"{e.ActionDescriptor.RouteValues["controller"]}{e.ActionDescriptor.AttributeRouteInfo?.Name}");
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sprinti Api", Version = "v1"
            });
        });
    }
}