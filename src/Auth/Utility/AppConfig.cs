using System.Text;
using Microsoft.Extensions.Configuration;

namespace Auth.Utility
{
    public class AppConfig
    {
        public DatabaseConfig DatabaseConfig { get; set; }

        public AuthConfig AuthConfig { get; set; }

        public AdminCredentials AdminCredentials { get; set; }

        public bool AllowInsecureHttp { get; set; }

        public SMTPConfig SMTPConfig { get; set; }

        public AppConfig(IConfigurationRoot configuration)
        {
            DatabaseConfig = new DatabaseConfig
            {
                Host = configuration.GetValue<string>("DB_HOST"),
                Username = configuration.GetValue<string>("DB_USERNAME"),
                Password = configuration.GetValue<string>("DB_PASSWORD"),
                Name = configuration.GetValue<string>("DB_NAME")
            };

            AuthConfig = new AuthConfig 
            {
                ClientId = configuration.GetValue<string>("CLIENT_ID"),
                Domain = configuration.GetValue<string>("DOMAIN")
            };

            AdminCredentials = new AdminCredentials
            {
                Email = configuration.GetValue<string>("ADMIN_EMAIL"),
                Password = configuration.GetValue<string>("ADMIN_PASSWORD")
            };

            SMTPConfig = new SMTPConfig
            {
                Server = configuration.GetValue<string>("SMTP_SERVER"),
                Port = configuration.GetValue<int>("SMTP_PORT"),
                WithSSL = configuration.GetValue<bool>("SMTP_WITH_SSL"),
                User = configuration.GetValue<string>("SMTP_USER"),
                Password = configuration.GetValue<string>("SMTP_PASSWORD")
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

    public class AuthConfig
    {
        public string ClientId { get; set; }

        public string Domain { get; set; }
    }

    public class SMTPConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool WithSSL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
