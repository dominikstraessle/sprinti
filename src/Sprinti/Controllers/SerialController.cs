using Microsoft.AspNetCore.Mvc;
using Sprinti.Domain;
using Sprinti.Serial;

namespace Sprinti.Controllers;

public class SerialController(ISerialService service) : ApiController
{
    [HttpPost(nameof(Raw), Name = nameof(Raw))]
    [ProducesResponseType(typeof(FinishedResponse), 202)]
    public async Task<IActionResult> Raw([FromQuery] string command, CancellationToken cancellationToken)
    {
        var response = await service.SendRawCommand(command, cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Rotate), Name = nameof(Rotate))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> Rotate([FromQuery] int degree, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new RotateCommand(degree), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Eject), Name = nameof(Eject))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> Eject([FromQuery] Color color, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new EjectCommand(color), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Lift), Name = nameof(Lift))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> Lift([FromQuery] Direction direction, CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new LiftCommand(direction), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Start), Name = nameof(Start))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> Start(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new StartCommand(), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(InitAlign), Name = nameof(InitAlign))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> InitAlign(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new InitCommand(), cancellationToken);
        if (response.ResponseState is not ResponseState.Complete) return Accepted(response);

        response = await service.SendCommand(new AlignCommand(), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Init), Name = nameof(Init))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> Init(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new InitCommand(), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Align), Name = nameof(Align))]
    [ProducesResponseType(typeof(CompletedResponse), 202)]
    public async Task<IActionResult> Align(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new AlignCommand(), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(Finish), Name = nameof(Finish))]
    [ProducesResponseType(typeof(FinishedResponse), 202)]
    public async Task<IActionResult> Finish(CancellationToken cancellationToken)
    {
        var response = await service.SendCommand(new FinishCommand(), cancellationToken);
        return Accepted(response);
    }

    [HttpPost(nameof(RunInstructionsAndFinish), Name = nameof(RunInstructionsAndFinish))]
    [ProducesResponseType(typeof(int), 202)]
    public async Task<IActionResult> RunInstructionsAndFinish(CancellationToken cancellationToken)
    {
        var powerInWattHours = await service.RunInstructionsAndFinish(new List<ISerialCommand>
        {
            new StartCommand(),
            new RotateCommand(90),
            new EjectCommand(Color.Yellow),
            new RotateCommand(-90),
            new EjectCommand(Color.Yellow),
            new EjectCommand(Color.Blue),
            new RotateCommand(180),
            new EjectCommand(Color.Red),
            new RotateCommand(180),
            new EjectCommand(Color.Blue),
            new LiftCommand(Direction.Down),
            new FinishCommand()
        }, cancellationToken);
        return Accepted(powerInWattHours);
    }
}