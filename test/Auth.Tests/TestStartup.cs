using Auth.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            // services.AddDbContext<ApplicationDbContext>(opts => opts.UseInMemoryDatabase());
        }
    }
}
