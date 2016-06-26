using Microsoft.Extensions.Configuration;

namespace Auth.Utility
{
    public class AppConfig
    {
        public bool AllowInsecureHttp { get; set; }

        public AdminCredentials AdminCredentials { get; set; }

        public AppConfig(IConfigurationRoot configuration)
        {
            AllowInsecureHttp = configuration.GetValue<bool>("Configurations:AllowInsecureHttp");
            
            AdminCredentials = new AdminCredentials
            {
                Email = configuration.GetValue<string>("Configurations:Admin:Email"),
                Password = configuration.GetValue<string>("Configurations:Admin:Password")
            };
        }
    }

    public class AdminCredentials
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
