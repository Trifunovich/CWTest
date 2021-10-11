using Autofac;
using Autofac.Configuration;
using CWTest.Core.Configurations;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Services;
using DataAccess.CWTest.Abstraction;
using DataAccess.Excel.Context;
using DataAccess.Excel.ExcelManipulation;
using DataAccess.Excel.Models;
using DataAccess.Excel.Repositories;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Excel.Services
{
    public static class ServiceCollectionExtension
    {
        public static ContainerBuilder AddExcelAccessInternals(this ContainerBuilder builder)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            builder.RegisterType<ExcelContext>().As<IExcelContext>().SingleInstance();
            builder.RegisterType<ExcelExtractor>().As<IExtractor>().SingleInstance();
            builder.RegisterType<ExcelConfiguration>().As<IExcelConfiguration>().SingleInstance();
            builder.RegisterType(typeof(ComponentsDataRepository)).As<IDataRepository<IComponentSpecification>>();

            builder.AddDataAccessCoreInternals();

            return builder;
        }

    }
}