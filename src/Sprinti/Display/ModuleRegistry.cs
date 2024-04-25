using System.Device.I2c;
using Iot.Device.Graphics;
using Iot.Device.Ssd13xx;
using Microsoft.Extensions.Options;

namespace Sprinti.Display;

public static class ModuleRegistry
{
    public static void AddDisplayModule(this IServiceCollection services)
    {
        services.AddScoped<I2cDevice>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<DisplayOptions>>().Value;
            return I2cDevice.Create(new I2cConnectionSettings(options.BusId, options.Address));
        });
        services.AddScoped<GraphicDisplay, Ssd1306>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<DisplayOptions>>().Value;
            return new Ssd1306(provider.GetRequiredService<I2cDevice>(), options.Width, options.Height);
        });
        services.AddScoped<IDisplayService, DisplayService>();
    }
}