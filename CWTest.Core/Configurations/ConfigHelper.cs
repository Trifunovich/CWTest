using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CWTest.Core.Configurations
{
    public static class ConfigHelper
    {
        public static IConfiguration Configure(string fileName)
        {
            string dir = Directory.GetCurrentDirectory();
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile(fileName);
            IConfigurationRoot built = builder.Build();
            return built;
        }
    }
}
