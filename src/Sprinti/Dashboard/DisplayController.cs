using Microsoft.AspNetCore.Mvc;
using Sprinti.Display;

namespace Sprinti.Dashboard;

public class DisplayController(
    IDisplayService displayService
) : ApiController
{
    [HttpGet(nameof(DebugText), Name = nameof(DebugText))]
    [ProducesResponseType(200)]
    public Task<IActionResult> DebugText([FromQuery] string text = "Hello World")
    {
        displayService.Print(text);
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet(nameof(UpdateProgress), Name = nameof(UpdateProgress))]
    [ProducesResponseType(200)]
    public Task<IActionResult> UpdateProgress([FromQuery] string text = "Hello World")
    {
        displayService.UpdateProgress(text);
        return Task.FromResult<IActionResult>(Ok());
    }
}