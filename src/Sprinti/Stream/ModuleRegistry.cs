namespace Sprinti.Api.Stream;

public static class ModuleRegistry
{
    public static void AddStreamModule(this IServiceCollection services)
    {
        services.AddHostedService<VideoStream>();
    }
}