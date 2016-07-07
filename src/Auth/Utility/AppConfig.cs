using System.Text;
using Microsoft.Extensions.Configuration;

namespace Auth.Utility
{
    public class AppConfig
    {
        public DatabaseConfig DatabaseConfig { get; set; }

        public AdminCredentials AdminCredentials { get; set; }

        public bool AllowInsecureHttp { get; set; }

        public AppConfig(IConfigurationRoot configuration)
        {
            DatabaseConfig = new DatabaseConfig
            {
                Host = configuration.GetValue<string>("DB_HOST"),
                Username = configuration.GetValue<string>("DB_USERNAME"),
                Password = configuration.GetValue<string>("DB_PASSWORD"),
                Name = configuration.GetValue<string>("DB_NAME")
            };

            AdminCredentials = new AdminCredentials
            {
                Email = configuration.GetValue<string>("ADMIN_EMAIL"),
                Password = configuration.GetValue<string>("ADMIN_PASSWORD")
            };

            AllowInsecureHttp = configuration.GetValue<bool>("ALLOW_HTTP");
        }
    }

    public class DatabaseConfig
    {
        public string Name { get; set; }

        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConnectionString { 
            get {
                StringBuilder connectionString = new StringBuilder();
                
                connectionString.Append($"Host={Host};");
                connectionString.Append($"Username={Username};");
                connectionString.Append($"Password={Password};");
                connectionString.Append($"Database={Name}");
                
                return connectionString.ToString();
            }
        }
    }

    public class AdminCredentials
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
