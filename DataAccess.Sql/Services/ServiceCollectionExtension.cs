using System;
using System.Collections.Generic;
using Autofac;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Services;
using DataAccess.CWTest.Abstraction;
using DataAccess.Sql.Context;
using DataAccess.Sql.Helpers;
using DataAccess.Sql.Models;
using DataAccess.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataAccess.Sql.Services
{
    public static class ServiceCollectionExtension
    {
        public static ContainerBuilder AddSqlDataAccessInternals(this ContainerBuilder builder, DataAccessRegistrationType daType)
        {
            builder.RegisterType<EfContextFactory>().As<IEfContextFactory>().SingleInstance();
            builder.RegisterType(typeof(SqlConnectionFactory)).As(typeof(ISqlConnectionFactory)).SingleInstance();
            builder.RegisterGeneric(typeof(DapperResolver<>)).As(typeof(IDapperResolver<>)).SingleInstance();

            RegisterContext<SqlEfContext>(builder);

            switch (daType)
            {
                case DataAccessRegistrationType.Loading:
                    builder.RegisterLoadingRepos();
                    break;
                case DataAccessRegistrationType.Storing:
                    builder.RegisterStoringRepos();
                    break;
                case DataAccessRegistrationType.All:
                    builder.RegisterLoadingRepos();
                    builder.RegisterStoringRepos();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(daType), daType, null);
            }

            builder.AddDataAccessCoreInternals();

            return builder;
        }

        private static void RegisterLoadingRepos(this ContainerBuilder builder)
        {
            builder.RegisterType(typeof(LoadingComponentsDbRepository)).As<ILoadingDataRepository<IComponentSpecification>>();
        }
        private static void RegisterStoringRepos(this ContainerBuilder builder)
        {
            builder.RegisterType(typeof(StoringComponentsDbRepository)).As<IStoringDataRepository<IComponentSpecification>>();
        }

        private static void RegisterContext<TContext>(ContainerBuilder builder)
          where TContext : DbContext
        {
            builder.Register(componentContext =>
              {
                  var dbContextOptions = new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                  var optionsBuilder = new DbContextOptionsBuilder<TContext>(dbContextOptions)
              .UseSqlServer(ConnectionHelper.SqlConnectionString);

                  return optionsBuilder.Options;
              }).As<DbContextOptions<TContext>>()
              .InstancePerLifetimeScope();

            builder.Register(context => context.Resolve<DbContextOptions<TContext>>())
              .As<DbContextOptions>()
              .InstancePerLifetimeScope();

            builder.RegisterType<TContext>()
              .AsSelf()
              .InstancePerLifetimeScope();
        }
    }
}