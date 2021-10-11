using System;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using CWTest.Core.Configurations;
using DataAccess.Excel.Services;
using DataAccess.Sql.Services;
using DataServiceProvider.TestBench.Services;
using Microsoft.Extensions.Configuration;

namespace DataServiceProvider.TestBench.DependencyInjection
{
    public static class DiReslverHelper
    {
        public static ContainerBuilder RegisterDataServiceProviderDependencies(this ContainerBuilder builder)
        {
            DataSourceType type = GetDataSourceType();

            switch (type)
            {
                case DataSourceType.Sql:
                    builder.AddSqlDataAccessInternals();
                    break;
                case DataSourceType.Excel:
                    builder.AddExcelAccessInternals();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.RegisterType<ComponentsService>().As<IComponentsService>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAutoMapper(assemblies);
            return builder;
        }

        private static DataSourceType GetDataSourceType()
        {
            IConfiguration config = ConfigHelper.Configure("DataServiceProviderConfig.json");
            DataSourceType typeVal = config.GetValue<DataSourceType>(nameof(DataSourceType));
            return typeVal;
        }
    }
}
