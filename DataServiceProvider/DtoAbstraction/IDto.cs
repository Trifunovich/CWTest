using CWTest.Core.DataManipulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceProvider.Core.DtoAbstraction
{
  /// <summary>
  /// Data transfer object abstraction
  /// </summary>
  public interface IDto
  {
    IDAbstraction Id { get;}
    string Label { get; set; }
    bool IsActive { get; set; }
    DateTime CreatedOn { get; set; }
    DateTime UpdatedOn { get; set; }
  }
}
