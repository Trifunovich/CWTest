using DataServiceProvider.Core.DtoAbstraction;
using DataServiceProvider.UnitOfWork;
using LoggingLibrary;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataServiceProvider.Core.UnitOfWork
{
  public abstract class UnitOfWorkBase<T, TId> : IUnitOfWork<T, TId> where T : IDto<TId>
  {
    private IList<T> _dirtyList = new List<T>();
    private readonly IBasicLogger<UnitOfWorkBase<T, TId>> _logger;

    public UnitOfWorkBase (IBasicLogger<UnitOfWorkBase<T, TId>> logger)
    {
      _logger = logger;
    }

    public bool RegisterClean(T item)
    {
      return RegisterAndLog(item, _dirtyList, "Uow element cleaning");
    }

    private bool RegisterAndLog(T item, IList<T> list, string logMessage)
    {
      bool result = true;

      if (list.Contains(item))
      {
        result = list.Remove(item);
      }

      LogSuccess(result, "UoW element cleaning");
      return result;
    }


    private void LogSuccess(bool successfull, string description)
    {
      if (successfull)
      {
        _logger.LogDebug("Successfully done {0}", description);
      }
      else 
      {
        _logger.LogDebug("Failed to do {0}", description);
      }
    }

    public bool RegisterDirty(T item, IdAbstraction<TId> id)
    {
      if (!_dirtyList.Contains(item))
      {
        _dirtyList.Add(item);
        return true;
      }

      return false;
    }

    public bool RegisterInsert(IEnumerable<T> records)
    {
      throw new NotImplementedException();
    }

    public bool RegisterRemove(IdAbstraction<TId> id, bool softRemove)
    {
      throw new NotImplementedException();
    }

    public Task<UoWRegisterResult> CleanChanges()
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAll(bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAll(Predicate<T> filter)
    {
      throw new NotImplementedException();
    }

    public Task<T> GetFirst(Predicate<T> filter)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive = true)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, Predicate<T> filter)
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
