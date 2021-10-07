using Autofac;
using CwTest.Ui.Shared.DependencyInjection;
using CWTest.Ui.WPF.ViewModel.Controls;

namespace CWTest.Ui.WPF.DependencyInjection
{
  internal class Bootstrapper : AutofacBootstrapper<ShellViewModel>
  {
    /// <summary>
    /// Override to add your own types to the IoC container.
    /// </summary>
    protected override void ConfigureIoC(ContainerBuilder builder)
    {
      base.ConfigureIoC(builder);     
    }
  }
}
