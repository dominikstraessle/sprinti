using Iot.Device.Board;
using Iot.Device.Button;
using Microsoft.Extensions.Options;

namespace Sprinti.Button;

public static class ModuleRegistry
{
    public static void AddButtonModule(this IServiceCollection services)
    {
        // The button must be transient and only used a single time. Otherwise the
        services.AddSingleton<Board>(_ => Board.Create());
        services.AddScoped<ButtonBase>(provider => new GpioButton(
            provider.GetRequiredService<IOptions<ButtonOptions>>().Value.Pin,
            isPullUp: provider.GetRequiredService<IOptions<ButtonOptions>>().Value.IsPullUp,
            hasExternalResistor: provider.GetRequiredService<IOptions<ButtonOptions>>().Value.UseExternalResistor,
            gpio: provider.GetRequiredService<Board>().CreateGpioController()
        ));
        services.AddScoped<IButtonService, ButtonService>();
    }
}