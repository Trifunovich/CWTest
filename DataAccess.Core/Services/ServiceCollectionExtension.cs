using Autofac;
using DataAccess.Core.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Core.Services
{
  public static class ServiceCollectionExtension
  {
    public static ContainerBuilder AddDataAccessCoreInternals(this ContainerBuilder builder)
    {
      builder.RegisterType<RepositoryInputValidator>().As<IRepositoryInputValidator>().SingleInstance();
      return builder;
    }
  }
}