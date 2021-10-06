using Autofac;
using CWTest.Core.DependencyInjection;
using Stylet;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CWTest.Ui.WPF.DependencyInjection
{
  public class AutofacBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
  {
    private IContainer container;

    private object _rootViewModel;
    protected virtual object RootViewModel
    {
      get { return this._rootViewModel ?? (this._rootViewModel = this.GetInstance(typeof(TRootViewModel))); }
    }

    protected override void ConfigureBootstrapper()
    {
      var builder = new ContainerBuilder();
      this.DefaultConfigureIoC(builder);
      this.ConfigureIoC(builder);
      this.container = builder.Build();
    }

    /// <summary>
    /// Carries out default configuration of the IoC container. Override if you don't want to do this
    /// </summary>
    protected virtual void DefaultConfigureIoC(ContainerBuilder builder)
    {
      var viewManagerConfig = new ViewManagerConfig()
      {
        ViewFactory = this.GetInstance,
        ViewAssemblies = new List<Assembly>() { this.GetType().Assembly }
      };
      builder.RegisterInstance<IViewManager>(new ViewManager(viewManagerConfig));

      builder.RegisterInstance<IWindowManagerConfig>(this).ExternallyOwned();
      builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
      builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
      builder.RegisterType<MessageBoxViewModel>().As<IMessageBoxViewModel>().ExternallyOwned(); // Not singleton!
      builder.RegisterAssemblyTypes(this.GetType().Assembly).ExternallyOwned();
    }

    /// <summary>
    /// Override to add your own types to the IoC container.
    /// </summary>
    protected virtual void ConfigureIoC(ContainerBuilder builder) 
    {
      DiRoslerHelper.RegisterDependenciesAutomaticallyByMarker(builder);
    }

    public override object GetInstance(Type type)
    {
      if (container == null)
      {
        ConfigureBootstrapper();
      }
      return this.container.Resolve(type);
    }

    protected override void Launch()
    {
      base.DisplayRootView(this.RootViewModel);
    }

    public override void Dispose()
    {
      ScreenExtensions.TryDispose(this._rootViewModel);
      if (this.container != null)
        this.container.Dispose();

      base.Dispose();
    }
  }
}
