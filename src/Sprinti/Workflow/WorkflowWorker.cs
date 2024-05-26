namespace Sprinti.Workflow;

public class WorkflowWorker(IServiceScopeFactory factory, ILogger<WorkflowWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var token = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        while (!token.IsCancellationRequested)
            try
            {
                using var scope = factory.CreateScope();
                var workflowService = scope.ServiceProvider.GetRequiredService<IWorkflowService>();
                logger.LogInformation("Start init");
                await workflowService.StartAsync(stoppingToken);
                logger.LogInformation("Init completed");
                logger.LogInformation("Start workflow run");
                await workflowService.RunAsync(stoppingToken);
                logger.LogInformation("Workflow run completed");
                logger.LogInformation("Wait for end");
                await workflowService.EndAsync(stoppingToken);
                logger.LogInformation("Wait for end completed");
            }
            catch (Exception e)
            {
                logger.LogError("Error in workflow worker: {Error}", e);
            }
    }
}