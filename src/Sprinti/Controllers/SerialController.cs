using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Serial;

namespace Sprinti.Controllers;

public class SerialController(ISerialService service) : ApiController
{
    [HttpPost(nameof(Raw), Name = nameof(Raw))]
    [ProducesResponseType(typeof(FinishedResponse), 201)]
    public async Task<IActionResult> Raw([FromQuery] string command, CancellationToken cancellationToken)
    {
        var response = await service.SendRawCommand(command, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(Rotate), Name = nameof(Rotate))]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Rotate([FromQuery] int degree, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new RotateCommand(degree), cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(Eject), Name = nameof(Eject))]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Eject([FromQuery] Color color, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new EjectCommand(color), cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(Lift), Name = nameof(Lift))]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Lift([FromQuery] Direction direction, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new LiftCommand(direction), cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(Reset), Name = nameof(Reset))]
    [ProducesResponseType(typeof(CompletedResponse), 201)]
    public async Task<IActionResult> Reset(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new ResetCommand(), cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(Finish), Name = nameof(Finish))]
    [ProducesResponseType(typeof(FinishedResponse), 201)]
    public async Task<IActionResult> Finish(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new FinishCommand(), cancellationToken);
        return Ok(response);
    }
}