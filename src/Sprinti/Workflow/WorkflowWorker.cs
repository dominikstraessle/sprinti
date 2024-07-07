namespace Sprinti.Workflow;

public class WorkflowWorker(IServiceScopeFactory factory, ILogger<WorkflowWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var token = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        while (!token.IsCancellationRequested)
            try
            {
                using var scope = factory.CreateScope();
                var workflowService = scope.ServiceProvider.GetRequiredService<IWorkflowService>();
                await workflowService.StartAsync(stoppingToken);
                await workflowService.RunAsync(stoppingToken);
                await workflowService.EndAsync(stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in workflow worker: {Exception}", e);
            }
    }
}