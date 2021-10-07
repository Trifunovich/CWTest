using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CWTest.Core.DataManipulation;
using DataServiceProvider.Core.UnitOfWork;
using DataServiceProvider.TestBench.Dtos;
using LoggingLibrary;

namespace DataServiceProvider.TestBench.Services
{
  public interface IComponentsService : IUnitOfWork<ComponentsDto>
  {
  }

  internal class ComponentsService : UnitOfWorkBase<ComponentsDto>
  {
    public ComponentsService(IBasicLogger<ComponentsService> logger) : base(logger)
    {

    }

    public override Task<IEnumerable<ComponentsDto>> GetPage(PagingParameters pagingParameters, Predicate<ComponentsDto> filter)
    {
      throw new NotImplementedException();
    }

    public override Task<UoWAggregatedResult> RevertAll()
    {
      throw new NotImplementedException();
    }

    public override Task<UoWAggregatedResult> SaveChanges()
    {
      throw new NotImplementedException();
    }

    public override Task<IEnumerable<ComponentsDto>> GetAll(bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public override Task<IEnumerable<ComponentsDto>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public override Task<IEnumerable<ComponentsDto>> GetAll(Predicate<ComponentsDto> filter)
    {
      throw new NotImplementedException();
    }

    public override Task<ComponentsDto> GetFirst(Predicate<ComponentsDto> filter)
    {
      throw new NotImplementedException();
    }

    public override Task<IEnumerable<ComponentsDto>> GetPage(PagingParameters pagingParameters, bool? isActive = true)
    {
      throw new NotImplementedException();
    }
  }
}
