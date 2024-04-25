namespace Sprinti.Serial;

public static class ModuleRegistry
{
    public static void AddSerialModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        if (!ISprintiOptions.RegisterOptions<SerialOptions>(services, configuration, SerialOptions.Serial)) return;

        services.AddScoped<ISerialAdapter, SerialAdapter>();
        services.AddScoped<ISerialService, SerialService>();
        // services.AddHostedService<SerialConsole>();
    }
}