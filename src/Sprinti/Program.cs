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


        // socat -d -d pty,raw,echo=0 pty,raw,echo=0
        app.Run();
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

        builder.Services.Configure<ConfirmationOptions>(builder.Configuration.GetSection(ConfirmationOptions.Confirmation));
    }
}