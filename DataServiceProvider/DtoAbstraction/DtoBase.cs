using CWTest.Core.DataManipulation;
using System;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public abstract class DtoBase : IDto
  {
    public IDAbstraction IdAbstraction { get; protected set; }

    public int InternalId { get; set; }

    public virtual string Label { get; set; }

    public virtual bool IsActive { get; set; }

    public virtual DateTime CreatedOn { get; set; }
    public byte[] Timestamp { get; set; }
    public abstract void SetId(IDAbstraction idAbstraction);
  }
}
