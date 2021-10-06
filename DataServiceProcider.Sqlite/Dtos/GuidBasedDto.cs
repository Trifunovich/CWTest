

using DataServiceProvider.Core.DtoAbstraction;
using System;

namespace DataServiceProcider.Sqlite.Dtos
{
  public abstract class GuidBasedDto : DtoBase<Guid>
  {
    private Guid _id;
    public override IdAbstraction<Guid> Id { get => new GuidAbstraction(_id); }

    public GuidBasedDto(Guid id)
    {
      _id = id;
    }
  }
}
