using Microsoft.AspNetCore.Mvc;
using Sprinti.Button;

namespace Sprinti.Dashboard;

public class ButtonController(
    IButtonService buttonService
) : ApiController
{
    [HttpGet(nameof(WaitForSignal), Name = nameof(WaitForSignal))]
    [ProducesResponseType(200)]
    public async Task<IActionResult> WaitForSignal([FromQuery] int timeout = 10)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

        await buttonService.WaitForSignalAsync(cancellationTokenSource.Token);
        return Ok();
    }
}