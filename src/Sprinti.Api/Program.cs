using Sprinti.Instruction;
using Sprinti.Serial;

namespace Sprinti.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        ConfigureBuilder(builder);

        var app = builder.Build();


        // socat -d -d pty,raw,echo=0 pty,raw,echo=0
        app.Run();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddLogging();

        var serialOptions = builder.Configuration.GetSection(SerialOptions.Serial);
        builder.Services.Configure<SerialOptions>(serialOptions);
        var serialOptionsValue = serialOptions.Get<SerialOptions>();
        if (serialOptionsValue is { Enabled: true }) builder.Services.AddSerialModule();
        builder.Services.AddInstructionModule();
    }
}