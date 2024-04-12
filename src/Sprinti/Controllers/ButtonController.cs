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
}