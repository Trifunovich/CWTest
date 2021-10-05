using CWTest.Ui.WPF.DependencyInjection;
using CWTest.Ui.WPF.ViewModel.BaseAbstractions;

namespace CWTest.Ui.WPF.ViewModel.Controls
{
  public class ShellViewModel : ViewModelBase, IShellViewModel, IRegisterTransient
  {
    public ShellViewModel()
    {
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
