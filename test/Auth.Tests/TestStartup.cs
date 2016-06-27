using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env)
            : base(env)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // Register here the dependency injection services.
        }
    }
}
