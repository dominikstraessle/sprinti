using Sprinti.Display;

namespace Sprinti.Workflow;

public static class ModuleRegistry
{
    public static void AddWorkflowModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        if (!ISprintiOptions.RegisterOptions<DisplayOptions>(services, configuration, DisplayOptions.Display)) return;

        services.AddScoped<IWorkflowService, WorkflowService>();
    }
}