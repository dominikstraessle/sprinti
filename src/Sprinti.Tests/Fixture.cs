using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sprinti.Serial;

namespace Sprinti.Tests
{
    public abstract class Fixture : IDisposable
    {
        private readonly IServiceScope _scope;

        protected Fixture()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Services.AddSerialModule();
            builder.Services.AddLogging();
            builder.Services.AddTestModule();
            AddTestServices(builder);
            var host = builder.Build();
            _scope = host.Services.CreateScope();
        }
        protected IServiceProvider ServiceProvider => _scope.ServiceProvider;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _scope.Dispose();
        }
        protected virtual void AddTestServices(HostApplicationBuilder builder)
        {
        }
    }

}