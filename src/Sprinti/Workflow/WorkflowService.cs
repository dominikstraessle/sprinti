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
        displayService.UpdateProgress(0, "start");
        await buttonService.WaitForSignalAsync(cancellationToken);
        displayService.UpdateProgress(1, "button pressed");
        await confirmationService.StartAsync(cancellationToken);
        displayService.UpdateProgress(2, "sent start");
        var config = videoService.RunDetection(cancellationToken);
        if (config is null)
        {
            throw new ArgumentException("NOOOOOOOO");
        }

        displayService.UpdateProgress(3, "detected formation");
        await confirmationService.ConfirmAsync(config, cancellationToken);
        displayService.UpdateProgress(4, "sent confirmation");
        var instructions = instructionService.GetInstructionSequence(config.Config);
        displayService.UpdateProgress(5, "calculated instruction sequence");
        var powerInWattHours = await serialService.RunInstructionsAndFinish(instructions, cancellationToken);
        displayService.UpdateProgress(6, $"instructions completed: consumed power {powerInWattHours}");
        await confirmationService.EndAsync(cancellationToken);
        displayService.UpdateProgress(7, $"sent end: consumed power {powerInWattHours}");
    }
}