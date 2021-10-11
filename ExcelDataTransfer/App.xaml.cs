using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Windows;
using Autofac;
using DataServiceProvider.TestBench.DependencyInjection;
using LoggingLibrary.DiResolver;

namespace ExcelDataTransfer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //don't do it, this is just a mini app, but this is a hack, not recommended at all for serious projects
        internal static IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterDataServiceProviderDependencies();
            builder.AddLoggingServices();
            Container = builder.Build();
        }
    }
}
