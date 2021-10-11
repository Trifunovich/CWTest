using CWTest.Core.DataManipulation;
using System;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public abstract class DtoBase : IDto
  {
    public IDAbstraction Id { get; set; }
    public int InternalId { get; set; }

    public virtual string Label { get; set; }

    public virtual bool IsActive { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime UpdatedOn { get; set; }
  }
}
