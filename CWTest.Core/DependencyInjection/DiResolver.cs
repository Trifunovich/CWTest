using Autofac;
using LoggingLibrary.DiResolver;
using System.Reflection;

namespace CWTest.Core.DependencyInjection
{
  public static class DiRoslerHelper
  {
    /// <summary>
    /// Registers all the dependencies in the assembly marked with appropriate interfaces
    /// </summary>
    /// <param name="builder">Container builder from Autofac</param>
    /// <returns>Updated container builder</returns>
    public static ContainerBuilder RegisterDependenciesAutomaticallyByMarker(this ContainerBuilder builder)
    {
      var assembly = typeof(DiRoslerHelper)
          .GetTypeInfo()
          .Assembly;

      builder.RegisterAssemblyTypes(assembly)
              .Where(x => x.IsAssignableTo<IRegisterSingleton>())
              .AsImplementedInterfaces().SingleInstance();

      builder.RegisterAssemblyTypes(assembly)
           .Where(x => x.IsAssignableTo<IRegisterTransient>())
           .AsImplementedInterfaces().InstancePerLifetimeScope();

      builder.AddLoggingServices();

      return builder;
    }
  }
}
