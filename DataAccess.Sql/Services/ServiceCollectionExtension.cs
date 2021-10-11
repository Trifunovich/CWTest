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
    private static void ResolveRepos<T>(ContainerBuilder builder) where T : SqlDataModelBase
    {
      switch (typeof(T))
      {
        case { } componentModel when componentModel == typeof(ComponentSpecification):
          builder.RegisterType(typeof(ComponentsDataRepository)).As<IDataRepository<IComponentSpecification>>();
          break;
        default:
          builder.RegisterType(typeof(EfDataRepository<T>)).As<IDataRepository<T>>();
          break;
      }
    }

    public static ContainerBuilder AddSqlDataAccessInternals(this ContainerBuilder builder)
    {
      builder.RegisterType<EfContextFactory>().As<IEfContextFactory>().SingleInstance();
      builder.RegisterType(typeof(SqlConnectionFactory)).As(typeof(ISqlConnectionFactory)).SingleInstance();
      builder.RegisterGeneric(typeof(DapperResolver<>)).As(typeof(IDapperResolver<>)).SingleInstance();
      
      RegisterContext<SqlEfContext>(builder);
      
      ResolveRepos<ComponentSpecification>(builder);

      builder.AddDataAccessCoreInternals();

      return builder;
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