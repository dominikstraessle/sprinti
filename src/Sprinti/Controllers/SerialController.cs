using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Serial;

namespace Sprinti.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SerialController(ISerialService service) : ControllerBase
{
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(FinishedResponse), 201)]
    public async Task<IActionResult> Raw([FromQuery] string command, CancellationToken cancellationToken)
    {
        var response = await service.SendRawCommand(command, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Route("rotate")]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Rotate([FromQuery] int degree, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new RotateCommand(degree), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Route("eject")]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Eject([FromQuery] Color color, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new EjectCommand(color), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Route("lift")]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Lift([FromQuery] Direction direction, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new LiftCommand(direction), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Route("reset")]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Reset(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new ResetCommand(), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Route("finish")]
    [ProducesResponseType(typeof(FinishedResponse), 201)]
    public async Task<IActionResult> Finish(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new FinishCommand(), cancellationToken);
        return Ok(response);
    }
}