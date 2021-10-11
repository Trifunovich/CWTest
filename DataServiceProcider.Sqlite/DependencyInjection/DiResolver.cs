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
            var types = GetDataSourceType();

            var loadType = types.Item1;

            if (loadType == DataSourceType.Excel)
            {
                builder.AddExcelAccessInternals();
                builder.AddSqlDataAccessInternals(DataAccessRegistrationType.Storing);
            }
            else
            {
                builder.AddSqlDataAccessInternals(DataAccessRegistrationType.All);
            }

            builder.RegisterType<ComponentsService>().As<IComponentsService>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAutoMapper(assemblies);
            return builder;
        }

        private static Tuple<DataSourceType, DataSourceType> GetDataSourceType()
        {
            IConfiguration config = ConfigHelper.Configure(@"appsettings.json");
            DataSourceType loadVal = config.GetValue<DataSourceType>("SystemForLoading");
            DataSourceType saveVal = config.GetValue<DataSourceType>("SystemForSaving");
            return new Tuple<DataSourceType, DataSourceType>(loadVal, saveVal);
        }
    }
}
