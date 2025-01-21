using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AdoNetCorePractica.Helper
{
    public class HelperConfiguration
    {
        public static string GetConnectionString()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("SqlTajamar");
            return connectionString;
        }
    }
}
