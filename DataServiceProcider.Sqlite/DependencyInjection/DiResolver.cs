
using Autofac;
using DataAccess.Core.Services;
using DataAccess.Sql.Services;

namespace DataServiceProvider.TestBench.DependencyInjection
{
  public static class DiRoslerHelper
  {
    public static ContainerBuilder RegisterDataServiceProviderDependencies(this ContainerBuilder builder)
    {
      builder.AddSqlDataAccessInternals();
      return builder;
    }
  }
}
