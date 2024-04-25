namespace Sprinti;

public interface ISprintiOptions
{
    bool Enabled { get; set; }

    public static bool RegisterOptions<T>(IServiceCollection services, ConfigurationManager configuration, string key)
        where T : class, ISprintiOptions
    {
        var section = configuration.GetSection(key);
        services.Configure<T>(section);
        var options = section.Get<T>();
        return options is { Enabled: true };
    }
}