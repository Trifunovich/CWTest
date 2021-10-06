using DataServiceProvider.Core.DtoAbstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceProcider.Sqlite.Dtos
{
  public class ComponentsDto : GuidBasedDto
  {
    public ComponentsDto(Guid id) : base(id)
    {

    }
  }
}
