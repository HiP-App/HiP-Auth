using System.Text;
using Microsoft.Extensions.Configuration;
using System;

namespace Auth.Utility
{
    public class AppConfig
    {
        public bool AllowInsecureHttp { get; set; }

        public AdminCredentials AdminCredentials { get; set; }

        public ConnectionString ConnectionString { get; set; }

        public AppConfig(IConfigurationRoot configuration)
        {
            AllowInsecureHttp = configuration.GetValue<bool>("Configurations:AllowInsecureHttp");
            
            if (configuration.GetValue<string>("ADMIN_EMAIL") != null)
            {
                AdminCredentials = new AdminCredentials
                {
                    Email = configuration.GetValue<string>("ADMIN_EMAIL"),
                    Password = configuration.GetValue<string>("ADMIN_PASSWORD")
                };
            }
            else
            {
                AdminCredentials = new AdminCredentials
                {
                    Email = configuration.GetValue<string>("Configurations:Admin:Email"),
                    Password = configuration.GetValue<string>("Configurations:Admin:Password")
                };
            }

            if (configuration.GetValue<string>("DB_HOST") != null)
            {
                StringBuilder connectionString = new StringBuilder("Host = " + configuration.GetValue<string>("DB_HOST") + ";");
                connectionString.Append("Password="+ configuration.GetValue<string>("DB_PASSWORD") + ";");
                connectionString.Append("Username=" + configuration.GetValue<string>("DB_USER") + ";");
                connectionString.Append("Database=" + configuration.GetValue<string>("DB_NAME"));
                ConnectionString = new ConnectionString
                {
                    DefaultConnection = connectionString.ToString()
                };
            }
            else
            {
                ConnectionString = new ConnectionString
                {
                    DefaultConnection = configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
                };
            }
            // Log
            Console.WriteLine(ConnectionString.DefaultConnection);
            Console.WriteLine(AdminCredentials.Email);
        }
    }

    public class ConnectionString
    {
        public string DefaultConnection { get; set; }
    }

    public class AdminCredentials
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
