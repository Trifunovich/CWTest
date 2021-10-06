using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public class GuidAbstraction : IdAbstraction<Guid>
  {
    private Guid _id;
    public Guid Value => _id;

    public GuidAbstraction(Guid id)
    {
      _id = id;
    }
  }
}
