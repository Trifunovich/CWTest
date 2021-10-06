using CWTest.Core.DependencyInjection;
using CWTest.Ui.WPF.ViewModel.BaseAbstractions;
using LoggingLibrary;
using Microsoft.Extensions.Logging;

namespace CWTest.Ui.WPF.ViewModel.Controls
{
  public class ShellViewModel : ViewModelBase, IShellViewModel, IRegisterTransient
  {
    public ShellViewModel(IBasicLogger<ShellViewModel> logger)
    {
      logger.LogInformation("ShellViewModel initialized");
      logger.LogDebug("Debug shell view model");
      logger.LogTrace("Trace shell view model");
    }

    public override string DisplayName
    {
      //todo: localization}
      get => "CW Testing";
    }


    protected override void OnInitialActivate()
    {
      base.OnInitialActivate();
    }
  }
}
