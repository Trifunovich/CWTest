using System.Diagnostics;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace LoggingLibrary.DiResolver
{
  public static class ServiceCollectionExtension
  {
    public static ContainerBuilder AddLoggingServices(this ContainerBuilder builder)
    {
      builder.RegisterInstance(new LoggerFactory()).As<ILoggerFactory>();
      builder.ResolveLogger();
      return builder;
    }

    public static ContainerBuilder ResolveLogger(this ContainerBuilder builder)
    {
      LoggerConfiguration config = new LoggerConfiguration().ReadFrom.Configuration(Configure("serilog.json"));
      builder.RegisterSerilog(config);
      builder.RegisterGeneric(typeof(LoggerWrapper<>)).As(typeof(IBasicLogger<>));
      return builder;
    }

    private static IConfiguration Configure(string fileName)
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