using System.Device.I2c;
using Iot.Device.Graphics;
using Iot.Device.Graphics.SkiaSharpAdapter;
using Iot.Device.Ssd13xx;
using Microsoft.Extensions.Options;

namespace Sprinti.Display;

public static class ModuleRegistry
{
    public static void AddDisplayModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        if (!ISprintiOptions.RegisterOptions<DisplayOptions>(services, configuration, DisplayOptions.Display)) return;

        // https://github.com/dotnet/iot/issues/2181#issuecomment-1833238952
        SkiaSharpAdapter.Register();

        services.AddScoped<I2cDevice>(provider =>
        {
            var displayOptions = provider.GetRequiredService<IOptions<DisplayOptions>>().Value;
            return I2cDevice.Create(new I2cConnectionSettings(displayOptions.BusId, displayOptions.Address));
        });
        services.AddScoped<GraphicDisplay, Ssd1306>(provider =>
        {
            var displayOptions = provider.GetRequiredService<IOptions<DisplayOptions>>().Value;
            return new Ssd1306(provider.GetRequiredService<I2cDevice>(), displayOptions.Width, displayOptions.Height);
        });
        services.AddScoped<IRenderService, RenderService>();
        services.AddScoped<IDisplayService, DisplayService>();
    }
}