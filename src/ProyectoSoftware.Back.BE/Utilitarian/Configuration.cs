using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Utilitarian
{
    public static class Configuration
    {
        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            return configuration;
        }
    }
}
