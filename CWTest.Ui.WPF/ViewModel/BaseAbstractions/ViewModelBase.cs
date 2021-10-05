using Stylet;
using System;

namespace CWTest.Ui.WPF.ViewModel.BaseAbstractions
{
  public abstract class ViewModelBase : Conductor<IViewModelBase>, IViewModelBase
  {
    private bool _disposedValue;
    private string _displayName;

    public virtual Guid Guid => GetType().GUID;

    public new virtual string DisplayName
    {
      get => _displayName ?? GetType().Name;    
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposedValue)
      {
        if (disposing)
        {
        }

        _disposedValue = true;
      }
    }      

    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}
