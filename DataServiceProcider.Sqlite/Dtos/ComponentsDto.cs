using System;
using DataServiceProvider.Core.DtoAbstraction;

namespace DataServiceProvider.TestBench.Dtos
{
  public class ComponentsDto : GuidBasedDto
  {
    public ComponentsDto(Guid id) : base(id)
    {

    }
  }
}
