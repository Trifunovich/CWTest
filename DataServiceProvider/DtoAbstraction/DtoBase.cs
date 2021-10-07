using CWTest.Core.DataManipulation;
using System;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public abstract class DtoBase : IDto
  {
    public abstract IDAbstraction Id { get;}

    public virtual string Label { get; set; }

    public virtual bool IsActive { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime UpdatedOn { get; set; }
  }
}
