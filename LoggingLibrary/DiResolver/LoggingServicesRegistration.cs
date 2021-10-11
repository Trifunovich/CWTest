using System.Diagnostics;
using System.IO;
using Autofac;
using LoggingLibrary.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace LoggingLibrary.DiResolver
{
  public static class LoggingServicesRegistration
  {
    public static ContainerBuilder AddLoggingServices(this ContainerBuilder builder)
    {
      builder.RegisterInstance(new LoggerFactory()).As<ILoggerFactory>();
      builder.ResolveLogger();
      return builder;
    }

    public static ContainerBuilder ResolveLogger(this ContainerBuilder builder)
    {
      LoggerConfiguration config = new LoggerConfiguration().ReadFrom.Configuration(ConfigHelper.Configure("serilog.json"));

      //we swap microsoft loggers for serilog
      builder.RegisterSerilog(config);
      builder.RegisterGeneric(typeof(LoggerWrapper<>)).As(typeof(IBasicLogger<>));
      return builder;
    }
  }
}