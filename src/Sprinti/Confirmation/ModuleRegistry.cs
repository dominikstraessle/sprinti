using Microsoft.Extensions.Options;

namespace Sprinti.Confirmation;

public static class ModuleRegistry
{
    public const string AuthHeaderName = "Auth";

    public static void AddConfirmationModule(this IServiceCollection services, ConfigurationManager configuration)
    {
        if (!ISprintiOptions.RegisterOptions<ConfirmationOptions>(services, configuration,
                ConfirmationOptions.Confirmation)) return;

        // Order is important: Register the adapter before the http client
        services.AddTransient<IConfirmationService, ConfirmationService>();
        services.AddHttpClient<IConfirmationService, ConfirmationService>((provider, client) =>
            ConfigureClient(provider.GetRequiredService<IOptions<ConfirmationOptions>>().Value, client));
    }

    internal static void ConfigureClient(ConfirmationOptions confirmationOptions, HttpClient client)
    {
        client.BaseAddress = confirmationOptions.BaseAddress;
        client.DefaultRequestHeaders.Add(AuthHeaderName, confirmationOptions.Password);
    }
}