using Microsoft.Extensions.Options;

namespace Sprinti.Confirmation;

public static class ModuleRegistry
{
    public static void AddConfirmationModule(this IServiceCollection services)
    {
        services.AddHttpClient<IConfirmationAdapter>((provider, client) =>
            ConfigureClient(provider.GetRequiredService<IOptions<ConfirmationOptions>>().Value, client));
        services.AddTransient<IConfirmationAdapter, ConfirmationAdapter>();
    }

    internal static void ConfigureClient(ConfirmationOptions confirmationOptions, HttpClient client)
    {
        client.BaseAddress = confirmationOptions.BaseAddress;
        client.DefaultRequestHeaders.Add("Auth", confirmationOptions.Password);
    }
}