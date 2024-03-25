using Microsoft.Extensions.Options;

namespace Sprinti.Confirmation;

public static class ModuleRegistry
{
    public static void AddConfirmationModule(this IServiceCollection services)
    {
        services.AddHttpClient<IConfirmationAdapter>(ConfigureClient);
        services.AddTransient<IConfirmationAdapter, ConfirmationAdapter>();
    }

    private static void ConfigureClient(IServiceProvider provider, HttpClient client)
    {
        var confirmationOptions = provider.GetRequiredService<IOptions<ConfirmationOptions>>().Value;
        client.BaseAddress = confirmationOptions.BaseAddress;
        client.DefaultRequestHeaders.Add("Auth", confirmationOptions.Password);
    }
}