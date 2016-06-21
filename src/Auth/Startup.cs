using Auth.Data;
using Auth.Models;
using Auth.Models.AuthViewModels;
using Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Models;
using System;
using System.IO;

namespace Auth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adding Cross Origin Requests 
            services.AddCors();

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddOpenIddictCore<Application>(config => config.UseEntityFramework());
           
            //Add Set the Admin Credentials from appsettings to a POCO Object
            services.Configure<LoginViewModel>(myoptions =>
            {
                myoptions.Email = Configuration.GetSection("AppCredentials:Admin:Username").Value;
                myoptions.Password = Configuration.GetSection("AppCredentials:Admin:Password").Value;
            });

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //swagger configurations
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, ApplicationDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                // This will allow any request from any server. Tweak to fit your needs!
                // The fluent API is pretty pleasant to work with.
                builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentity();

            app.UseOpenIddictCore(builder =>
            {
                // tell openiddict you're wanting to use jwt tokens
                builder.Options.UseJwtTokens();
                // NOTE: for dev consumption only! for live, this is not encouraged!
                builder.Options.AllowInsecureHttp = true;
                builder.Options.ApplicationCanDisplayErrors = true;
            });

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwaggerGen();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();

            //Call to create Admin
            StartupTasks user = new StartupTasks();
            user.CreateSuperuser(dbContext, serviceProvider, app.ApplicationServices.GetService<IOptions<LoginViewModel>>());
        }

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
