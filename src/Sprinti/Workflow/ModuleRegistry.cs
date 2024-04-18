namespace Sprinti.Workflow;

public static class ModuleRegistry
{
    public static void AddWorkflowModule(this IServiceCollection services)
    {
        services.AddScoped<IWorkflowService, WorkflowService>();
    }
}