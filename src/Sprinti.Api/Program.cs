using Sprinti.Serial;

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
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddLogging();
        var serialOptions = builder.Configuration.GetSection(SerialOptions.ConfigurationSectionName);
        var serialOptionsValue = serialOptions.Get<SerialOptions>();
        builder.Services.AddOptions<SerialOptions>()
            .Bind(serialOptions)
            .ValidateOnStart();
        if (serialOptionsValue is { Enabled: true })
        {
            builder.Services.AddSerialModule();
        }
    }
}