using Iot.Device.Board;
using Iot.Device.Button;
using Microsoft.Extensions.Options;

namespace Sprinti.Button;

public static class ModuleRegistry
{
    public static void AddButtonModule(this IServiceCollection services)
    {
        // The button must be transient and only used a single time. Otherwise the
        services.AddTransient<Board>(_ => Board.Create());
        services.AddTransient<ButtonBase>(provider => new GpioButton(
            provider.GetRequiredService<IOptions<ButtonOptions>>().Value.Pin,
            true,
            false,
            provider.GetRequiredService<Board>().CreateGpioController()
        ));
        services.AddTransient<IButtonService, ButtonService>();
    }
}