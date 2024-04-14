using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using Iot.Device.Button;
using Microsoft.Extensions.Options;

namespace Sprinti.Button;

public static class ModuleRegistry
{
    public static void AddButtonModule(this IServiceCollection services)
    {
        // pinctrl poll 26
        // gpioinfo -> use gpio chip 4 (https://github.com/dotnet/iot/issues/2262#issuecomment-1890441048)
        // use libgpiod v1 (v2 not supported at the moment) (https://github.com/dotnet/iot/issues/2179)
        // Pi5 Board not supported at the moment
        services.AddScoped<GpioDriver>(provider =>
            new LibGpiodDriver(provider.GetRequiredService<IOptions<ButtonOptions>>().Value.GpioChip));
        services.AddScoped<GpioController>(provider =>
            new GpioController(PinNumberingScheme.Logical, provider.GetRequiredService<GpioDriver>()));
        services.AddScoped<ButtonBase>(provider =>
        {
            var buttonOptions = provider.GetRequiredService<IOptions<ButtonOptions>>().Value;
            return new GpioButton(
                buttonOptions.Pin,
                buttonOptions.IsPullUp,
                buttonOptions.UseExternalResistor,
                provider.GetRequiredService<GpioController>()
            );
        });
        services.AddScoped<IButtonService, ButtonService>();
    }
}