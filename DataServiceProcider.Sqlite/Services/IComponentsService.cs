using DataServiceProcider.Sqlite.Dtos;
using DataServiceProvider.Core.DtoAbstraction;
using DataServiceProvider.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataServiceProcider.Sqlite.Services
{
  public interface IComponentsService : IUnitOfWork<ComponentsDto, Guid>
  {
  }

  internal class ComponentsService : IComponentsService
  {
    public Task<UoWRegisterResult> CleanChanges()
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ComponentsDto>> GetAll(bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ComponentsDto>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ComponentsDto>> GetAll(Predicate<ComponentsDto> filter)
    {
      throw new NotImplementedException();
    }

    public Task<ComponentsDto> GetFirst(Predicate<ComponentsDto> filter)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ComponentsDto>> GetPage(PagingParameters pagingParameters, bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ComponentsDto>> GetPage(PagingParameters pagingParameters, Predicate<ComponentsDto> filter)
    {
      throw new NotImplementedException();
    }

    public Task RegisterClean(ComponentsDto item)
    {
      throw new NotImplementedException();
    }

    public Task RegisterDirty(ComponentsDto item, IdAbstraction<Guid> id)
    {
      throw new NotImplementedException();
    }

    public Task RegisterInsert(IEnumerable<ComponentsDto> records)
    {
      throw new NotImplementedException();
    }

    public Task RegisterRemove(IdAbstraction<Guid> id, bool softRemove)
    {
      throw new NotImplementedException();
    }

    public Task<UoWAggregatedResult> RevertAll()
    {
      throw new NotImplementedException();
    }

    public Task<UoWAggregatedResult> SaveChanges()
    {
      throw new NotImplementedException();
    }
  }
}
