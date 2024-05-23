using Sprinti.Button;
using Sprinti.Confirmation;
using Sprinti.Detection;
using Sprinti.Display;
using Sprinti.Instruction;
using Sprinti.Serial;

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
        var startTask = confirmationService.StartAsync(cancellationToken);
        displayService.UpdateProgress(2, "sent start");
        var config = videoService.RunDetection(cancellationToken);
        if (config is null) throw new ArgumentException("NOOOOOOOO");

        displayService.UpdateProgress(3, "detected formation");
        var confirmTask = confirmationService.ConfirmAsync(config, cancellationToken);
        displayService.UpdateProgress(4, "sent confirmation");
        var instructions = instructionService.GetInstructionSequence(config.Config);
        displayService.UpdateProgress(5, "calculated instruction sequence");
        var powerInWattHours = await serialService.RunWorkflowProcedure(instructions, cancellationToken);
        displayService.UpdateProgress(6, $"instructions completed: consumed power {powerInWattHours}");
        var endTask = confirmationService.EndAsync(cancellationToken);
        await Task.WhenAll(startTask, confirmTask, endTask);
        displayService.UpdateProgress(7, $"{powerInWattHours / 3.6 * Math.Pow(10, 6)}Wh");
    }
}