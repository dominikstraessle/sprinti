using Iot.Device.Board;
using Iot.Device.Button;
using Microsoft.Extensions.Options;

namespace Sprinti.Button;

public static class ModuleRegistry
{
    public static void AddButtonModule(this IServiceCollection services)
    {
        // The button must be transient and only used a single time. Otherwise the
        services.AddTransient<Board>(provider =>
        {
            var board = Board.Create();
            board.CreateGpioController().OpenPin(provider.GetRequiredService<IOptions<ButtonOptions>>().Value.Pin);
            return board;
        });
        services.AddTransient<ButtonBase>(provider => new GpioButton(
            provider.GetRequiredService<IOptions<ButtonOptions>>().Value.Pin,
            provider.GetRequiredService<IOptions<ButtonOptions>>().Value.IsPullUp,
            provider.GetRequiredService<IOptions<ButtonOptions>>().Value.UseExternalResistor,
            provider.GetRequiredService<Board>().CreateGpioController()
        ));
        services.AddTransient<IButtonService, ButtonService>();
    }
}