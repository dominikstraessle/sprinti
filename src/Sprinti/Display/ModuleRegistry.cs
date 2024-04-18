namespace Sprinti.Display;

public static class ModuleRegistry
{
    public static void AddDisplayModule(this IServiceCollection services)
    {
        // var blub = I2cDevice.Create(new I2cConnectionSettings(1, 0x7C));
        services.AddScoped<IDisplayService, DisplayService>();
    }
}