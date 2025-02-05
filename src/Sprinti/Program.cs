using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Sprinti.Confirmation;
using Sprinti.Instruction;
using Sprinti.Serial;
using Sprinti.Stream;

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
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            c.RoutePrefix = "";
        });

        app.UseRouting();
        app.MapControllers();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        var serialOptions = builder.Configuration.GetSection(SerialOptions.Serial);
        builder.Services.Configure<SerialOptions>(serialOptions);
        var serialOptionsValue = serialOptions.Get<SerialOptions>();
        if (serialOptionsValue is { Enabled: true }) builder.Services.AddSerialModule();

        var streamOptions = builder.Configuration.GetSection(StreamOptions.Stream);
        builder.Services.Configure<StreamOptions>(streamOptions);
        var streamOptionsValue = streamOptions.Get<StreamOptions>();
        if (streamOptionsValue is { Enabled: true }) builder.Services.AddStreamModule();

        builder.Services.AddInstructionModule();

        builder.Services.Configure<ConfirmationOptions>(
            builder.Configuration.GetSection(ConfirmationOptions.Confirmation));
        builder.Services.AddConfirmationModule();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sprinti Api", Version = "v1" });
        });
    }
}