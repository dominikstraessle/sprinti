using Microsoft.AspNetCore.Mvc;
using Sprinti.Button;

namespace Sprinti.Controllers;

public class ButtonController(
    IButtonService buttonService
) : ApiController
{
    [HttpGet(nameof(WaitForSignal), Name = nameof(WaitForSignal))]
    [ProducesResponseType(200)]
    public async Task<IActionResult> WaitForSignal([FromQuery] int timeout)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

        await buttonService.WaitForSignalAsync(cancellationTokenSource.Token);
        return Ok();
    }

    // [HttpGet(nameof(TestButton), Name = nameof(TestButton))]
    // [ProducesResponseType(200)]
    // public Task<IActionResult> TestButton()
    // {
    //     using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    //
    //     var driver = new LibGpiodDriver(gpioChip: 4);
    //     var gpioController = new GpioController(PinNumberingScheme.Logical, driver);
    //     gpioController.OpenPin(options.Value.Pin, PinMode.InputPullUp);
    //     var oldValue = gpioController.Read(options.Value.Pin);
    //     while (!cancellationTokenSource.IsCancellationRequested)
    //     {
    //         var newValue = gpioController.Read(options.Value.Pin);
    //         if (oldValue != newValue)
    //         {
    //             return Task.FromResult<IActionResult>(Ok());
    //         }
    //     }
    //
    //     return Task.FromResult<IActionResult>(BadRequest());
    // }
}