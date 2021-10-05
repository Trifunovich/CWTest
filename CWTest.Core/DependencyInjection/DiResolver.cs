using Autofac;
using CWTest.Ui.WPF.ViewModel.Controls;
using System.Reflection;

namespace CWTest.Ui.WPF.DependencyInjection
{
  public static class ServiceCollectionExtension
  {
    public static ContainerBuilder AddWpfAutofacServices(this ContainerBuilder builder, string fileName)
    {
      var assembly = typeof(ServiceCollectionExtension)
          .GetTypeInfo()
          .Assembly;

      builder.RegisterAssemblyTypes(assembly)
              .Where(x => x.IsAssignableTo<IRegisterSingleton>())
              .AsImplementedInterfaces().SingleInstance();

      builder.RegisterAssemblyTypes(assembly)
           .Where(x => x.IsAssignableTo<IRegisterTransient>())
           .AsImplementedInterfaces().InstancePerLifetimeScope();

      return builder;
    }
  }
}
