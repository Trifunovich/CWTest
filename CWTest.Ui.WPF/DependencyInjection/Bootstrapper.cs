using Autofac;
using CWTest.Ui.WPF.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWTest.Ui.WPF.DependencyInjection
{
  class Bootstrapper : AutofacBootstrapper<ShellViewModel>
  {
    /// <summary>
    /// Override to add your own types to the IoC container.
    /// </summary>
    protected override void ConfigureIoC(ContainerBuilder builder)
    {
      base.ConfigureIoC(builder);
      //ServiceCollectionExtension.AddWpfAutofacServices(builder, "appsettings.json");
    }
  }
}
