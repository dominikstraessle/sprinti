namespace Sprinti.Workflow;

public static class ModuleRegistry
{
    public static void AddWorkflowModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        if (!ISprintiOptions.RegisterOptions<WorkflowOptions>(services, configuration, WorkflowOptions.Workflow))
            return;

        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddHostedService<WorkflowWorker>();
    }
}