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

public interface IDisplayService
{
    Task UpdateProgress(int stepNumber, string text, CancellationToken cancellationToken);
}

public class WorkflowService(
    IButtonService buttonService,
    IVideoService videoService,
    ISerialService serialService,
    IInstructionService instructionService,
    IConfirmationService confirmationService,
    IDisplayService displayService,
    ILogger<WorkflowService> logger)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await displayService.UpdateProgress(0, "start", cancellationToken);
        await buttonService.WaitForSignalAsync(cancellationToken);
        await displayService.UpdateProgress(1, "button pressed", cancellationToken);
        await confirmationService.StartAsync(cancellationToken);
        await displayService.UpdateProgress(2, "sent start", cancellationToken);
        var config = await videoService.DetectFormation(cancellationToken);
        await displayService.UpdateProgress(3, "detected formation", cancellationToken);
        await confirmationService.ConfirmAsync(new CubeConfig
        {
            Time = DateTime.Now,
            Config = config
        }, cancellationToken);
        await displayService.UpdateProgress(4, "sent confirmation", cancellationToken);
        var instructions = instructionService.GetInstructionSequence(config);
        await displayService.UpdateProgress(5, "calculated instruction sequence", cancellationToken);
        var powerInWattHours = await serialService.RunInstructionsAndFinish(instructions, cancellationToken);
        await displayService.UpdateProgress(6, $"instructions completed: consumed power {powerInWattHours}",
            cancellationToken);
        await confirmationService.EndAsync(cancellationToken);
        await displayService.UpdateProgress(7, "sent end", cancellationToken);
    }
}