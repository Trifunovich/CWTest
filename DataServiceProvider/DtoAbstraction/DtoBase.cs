using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public abstract class DtoBase<TIdAbs> : IDto<TIdAbs>
  {
    public abstract IdAbstraction<TIdAbs> Id { get;}

    public virtual string Label { get; set; }

    public virtual bool IsActive { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime UpdatedOn { get; set; }
  }
}
