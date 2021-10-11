using System.IO;
using Microsoft.Extensions.Configuration;

namespace LoggingLibrary.Configurations
{
    internal static class ConfigHelper
    {
        internal static IConfiguration Configure(string fileName)
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
