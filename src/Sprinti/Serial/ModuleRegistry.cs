namespace Sprinti.Serial;

public static class ModuleRegistry
{
    public static void AddSerialModule(this IServiceCollection services)
    {
        services.AddScoped<ISerialAdapter, SerialAdapter>();
        services.AddScoped<ISerialService, SerialService>();
        // services.AddHostedService<SerialConsole>();
    }
}