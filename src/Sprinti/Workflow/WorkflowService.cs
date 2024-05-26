using System.Diagnostics;
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
    Task StartAsync(CancellationToken cancellationToken);
    Task EndAsync(CancellationToken cancellationToken);
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
        displayService.Print("Heppo ist bereit für Start");
        await buttonService.WaitForSignalAsync(cancellationToken);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var startRequestTask = confirmationService.StartAsync(cancellationToken);
        displayService.UpdateProgress("Workflow gestartet");
        var config = videoService.RunDetection(cancellationToken);
        displayService.UpdateProgress("Erkennung gestartet");
        if (config is null)
        {
            logger.LogError("Failed to detect config");
            throw new ArgumentException("Failed to detect config");
        }

        var confirmTask = confirmationService.ConfirmAsync(config, cancellationToken);
        var instructions = instructionService.GetInstructionSequence(config.Config);
        displayService.UpdateProgress("Aufbau gestartet");
        var powerInWattHours = await serialService.RunWorkflowProcedure(instructions, cancellationToken);
        var endTask = confirmationService.EndAsync(cancellationToken);
        stopWatch.Stop();
        displayService.UpdateProgress($"""
                                       Workflow beendet
                                       Energie: {powerInWattHours:0.#####}Wh
                                       Zeit: {stopWatch.Elapsed.TotalSeconds}s
                                       Warten auf Server-Requests...
                                       """);
        await Task.WhenAll(startRequestTask, confirmTask, endTask);
        displayService.UpdateProgress($"""
                                       Workflow beendet
                                       Energie: {powerInWattHours:0.#####}Wh
                                       Zeit: {stopWatch.Elapsed.TotalSeconds}s
                                       Zum Beenden Knopf drücken...
                                       """);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        displayService.Print("Heppo ist bereit für Init");
        await buttonService.WaitForSignalAsync(cancellationToken);
        displayService.Print("Start-Prozedur wird gestartet");
        await serialService.RunStartProcedure(cancellationToken);
        displayService.Print("Start-Prozedur fertig");
    }

    public async Task EndAsync(CancellationToken cancellationToken)
    {
        await buttonService.WaitForSignalAsync(cancellationToken);
    }
}