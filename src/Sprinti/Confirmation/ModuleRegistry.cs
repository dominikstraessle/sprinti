namespace Sprinti.Confirmation;

public static class ModuleRegistry
{
    public static void AddConfirmationModule(this IServiceCollection services)
    {
        services.AddTransient<IConfirmationAdapter, ConfirmationAdapter>();
    }
}