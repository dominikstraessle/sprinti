using Sprinti.Button;
using Sprinti.Confirmation;
using Sprinti.Display;
using Sprinti.Instruction;
using Sprinti.Serial;
using Sprinti.Stream;

namespace Sprinti.Workflow;

public interface IWorkflowService
{
    Task RunAsync(CancellationToken cancellationToken);
}

public class WorkflowService(
    IButtonService buttonService,
    IVideoProcessor videoService,
    ISerialService serialService,
    IInstructionService instructionService,
    IConfirmationService confirmationService,
    IDisplayService displayService,
    ILogger<WorkflowService> logger) : IWorkflowService
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await displayService.UpdateProgress(0, "start", cancellationToken);
        await buttonService.WaitForSignalAsync(cancellationToken);
        await displayService.UpdateProgress(1, "button pressed", cancellationToken);
        await confirmationService.StartAsync(cancellationToken);
        await displayService.UpdateProgress(2, "sent start", cancellationToken);
        var config = videoService.RunDetection(cancellationToken);
        if (config is null)
        {
            throw new ArgumentException("NOOOOOOOO");
        }

        await displayService.UpdateProgress(3, "detected formation", cancellationToken);
        await confirmationService.ConfirmAsync(config, cancellationToken);
        await displayService.UpdateProgress(4, "sent confirmation", cancellationToken);
        var instructions = instructionService.GetInstructionSequence(config.Config);
        await displayService.UpdateProgress(5, "calculated instruction sequence", cancellationToken);
        var powerInWattHours = await serialService.RunInstructionsAndFinish(instructions, cancellationToken);
        await displayService.UpdateProgress(6, $"instructions completed: consumed power {powerInWattHours}",
            cancellationToken);
        await confirmationService.EndAsync(cancellationToken);
        await displayService.UpdateProgress(7, "sent end", cancellationToken);
    }
}