using System;
using CWTest.Core.DataManipulation;

namespace DataAccess.Core.Abstractions
{
  public interface IDataModel
  {
    IDAbstraction IdAbstraction { get; }
    int InternalId { get; }
    string Label { get; set; }
    bool IsActive { get; set; }
    DateTime CreatedOn { get; set; }
    DateTime UpdatedOn { get; set; }

    void SetId(IDAbstraction idAbstraction);
  }

  public interface IDataModelBase<T> : IDataModel
  {
    T Id { get; set; }
  }
}