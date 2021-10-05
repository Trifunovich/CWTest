using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWTest.Ui.WPF.ViewModel.BaseAbstractions
{
  public interface IViewModelBase : IDisposable
  {
    /// <summary>
    /// you can override it in the model with a known value or use gettype.guid for it
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    /// it is used for databinding in the view
    /// </summary>
    string DisplayName { get; }
  }
}
