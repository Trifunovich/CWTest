using System;
using CWTest.Core.DataManipulation;

namespace DataAccess.Core.Abstractions
{
  public interface IDataModel
  {
    IDAbstraction IdAbstraction { get; }
    string Label { get; set; }
    bool IsActive { get; set; }
    DateTime CreatedOn { get; set; }
    DateTime UpdatedOn { get; set; }
  }

  public interface IDataModelBase<T> : IDataModel
  {
    T Id { get; set; }
  }
}