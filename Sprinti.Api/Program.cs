using Sprinti.Api.Serial;

namespace Sprinti.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        ConfigureBuilder(builder);

        var app = builder.Build();


        // socat -d -d pty,raw,echo=0 pty,raw,echo=0
        app.Run();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<SerialOptions>()
            .Bind(builder.Configuration.GetSection(SerialOptions.ConfigurationSectionName))
            .ValidateOnStart();
        builder.Services.AddSerialModule();
        builder.Services.AddHostedService<SerialWorker>();
    }
}