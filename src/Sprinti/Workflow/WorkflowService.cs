using Sprinti.Confirmation;
using Sprinti.Domain;
using Sprinti.Instruction;
using Sprinti.Serial;

namespace Sprinti.Workflow;

public interface IButtonService
{
    Task WaitForSignalAsync(CancellationToken cancellationToken);
}

public interface IVideoService
{
    Task<SortedDictionary<int, Color>> DetectFormation(CancellationToken cancellationToken);
}

public class WorkflowService(
    IButtonService buttonService,
    IVideoService videoService,
    ISerialService serialService,
    IInstructionService instructionService,
    IConfirmationService confirmationService,
    ILogger<WorkflowService> logger)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await buttonService.WaitForSignalAsync(cancellationToken);
        await confirmationService.StartAsync(cancellationToken);
        var config = await videoService.DetectFormation(cancellationToken);
        await confirmationService.ConfirmAsync(new CubeConfig
        {
            Time = DateTime.Now,
            Config = config
        }, cancellationToken);
        var instructions = instructionService.GetInstructionSequence(config);
        var powerInWattHours = 0;
        foreach (var command in instructions)
        {
            if (command is FinishCommand finishCommand)
            {
                var response = await serialService.SendCommand(finishCommand, cancellationToken);
                powerInWattHours = response.PowerInWattHours;
            }
            else
            {
                var response = await serialService.SendCommand(command, cancellationToken);
            }
        }

        await confirmationService.EndAsync(cancellationToken);
    }
}