using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using Iot.Device.Button;
using Microsoft.Extensions.Options;

namespace Sprinti.Button;

public static class ModuleRegistry
{
    public static void AddButtonModule(this IServiceCollection services)
    {
        // The button must be transient and only used a single time. Otherwise the
        services.AddScoped<GpioDriver>(provider =>
            new LibGpiodDriver(gpioChip: provider.GetRequiredService<IOptions<ButtonOptions>>().Value.GpioChip));
        services.AddScoped<GpioController>(provider =>
            new GpioController(PinNumberingScheme.Logical, provider.GetRequiredService<GpioDriver>()));
        services.AddScoped<ButtonBase>(provider =>
        {
            var buttonOptions = provider.GetRequiredService<IOptions<ButtonOptions>>().Value;
            return new GpioButton(
                buttonOptions.Pin,
                isPullUp: buttonOptions.IsPullUp,
                hasExternalResistor: buttonOptions.UseExternalResistor,
                gpio: provider.GetRequiredService<GpioController>()
            );
        });
        services.AddScoped<IButtonService, ButtonService>();
    }
}