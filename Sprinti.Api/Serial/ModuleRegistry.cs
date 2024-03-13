using System.IO.Ports;

namespace Sprinti.Api.Serial;

public static class ModuleRegistry
{
    public static void AddSerialModule(this IServiceCollection services)
    {
        services.AddSingleton<SerialPort>();
        services.AddSingleton<ISerialAdapter, SerialAdapter>();
        services.AddSingleton<SerialService>();
    }
}