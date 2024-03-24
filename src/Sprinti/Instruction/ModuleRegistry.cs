namespace Sprinti.Api.Instruction;

public static class ModuleRegistry
{
    public static void AddInstructionModule(this IServiceCollection services)
    {
        services.AddTransient<IInstructionService, InstructionService>();
    }
}