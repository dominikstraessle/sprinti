using Microsoft.AspNetCore.Mvc;
using Sprinti.Display;

namespace Sprinti.Controllers;

public class DisplayController(
    IDisplayService displayService
) : ApiController
{
    [HttpGet(nameof(DebugText), Name = nameof(DebugText))]
    [ProducesResponseType(200)]
    public Task<IActionResult> DebugText([FromQuery] string text = "Hello World")
    {
        displayService.Debug(text);
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet(nameof(UpdateProgress), Name = nameof(UpdateProgress))]
    [ProducesResponseType(200)]
    public Task<IActionResult> UpdateProgress([FromQuery] int stepNumber = 0, [FromQuery] string text = "Hello World")
    {
        displayService.UpdateProgress(stepNumber, text);
        return Task.FromResult<IActionResult>(Ok());
    }
}