using CWTest.Core.DataManipulation;
using System;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public abstract class GuidBasedDto : DtoBase
  {
    private Guid _id;

    public override IDAbstraction Id { get => new GuidAbstraction(_id); }

    public GuidBasedDto(Guid id)
    {
      _id = id;
    }
  }
}
