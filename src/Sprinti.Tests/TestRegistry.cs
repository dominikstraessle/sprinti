using Microsoft.Extensions.DependencyInjection;

namespace Sprinti.Tests;

internal static class ModuleRegistry
{
    internal static void AddTestModule(this IServiceCollection services)
    {
        // INFO: Add mocks or other global test services
    }
}