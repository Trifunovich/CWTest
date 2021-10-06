using DataServiceProvider.Core.DtoAbstraction;
using DataServiceProvider.UnitOfWork;
using LoggingLibrary;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceProvider.Core.UnitOfWork
{
  public abstract class UnitOfWorkBase<T, TId> : IUnitOfWork<T, TId> where T : IDto<TId>
  {
    private IList<T> _dirtyList = new List<T>();
    private List<T> _insertList = new List<T>();
    private ConcurrentDictionary<IdAbstraction<TId>, bool> _deleteList = new ConcurrentDictionary<IdAbstraction<TId>, bool>();

    private readonly IBasicLogger<UnitOfWorkBase<T, TId>> _logger;

    public UnitOfWorkBase(IBasicLogger<UnitOfWorkBase<T, TId>> logger)
    {
      _logger = logger;
    }

    private bool RegisterAndLog(Func<bool> toDo, string logAction, string logItem)
    {
      bool result = toDo.Invoke();
      LogSuccess(result, logAction, logItem);
      return result;
    }


    private void LogSuccess(bool successfull, string descriptionAction, string desctriptionItem)
    {
      if (successfull)
      {
        _logger.LogDebug("Successfully {0}, item - {1}", descriptionAction, desctriptionItem);
      }
      else
      {
        _logger.LogDebug("Failed {0}, item - {1}", descriptionAction, desctriptionItem);
      }
    }

    public bool RegisterClean(T item)
    {
      return RegisterAndLog(
        () =>
        {
          if (_dirtyList.Contains(item))
          {
            return _dirtyList.Remove(item);
          }

          return true;
        }, "Uow element cleaning", item.Id.ValueAsString);
    }

    public bool RegisterDirty(T item, IdAbstraction<TId> id)
    {
      return RegisterAndLog(
       () =>
       {
         if (!_dirtyList.Contains(item))
         {
           _dirtyList.Add(item);
         }

         return true;
       }, "Uow element clean", item.Id.ValueAsString);     
    }

    public bool RegisterInsert(IEnumerable<T> records)
    {
      return RegisterAndLog(
      () =>
      {
        foreach (var x in records)
        {
          bool res = false;
          _deleteList.TryRemove(x.Id, out res);
          _dirtyList.Remove(x);
          _insertList.Remove(x);
        }

        _insertList.AddRange(records);

        return true;
      }, "Inserting elements into UoW cache", records.Count().ToString());
    }

    public bool RegisterRemove(IdAbstraction<TId> id, bool softRemove)
    {
      return RegisterAndLog(
     () =>
     {
       if (!_deleteList.Keys.Contains(id))
       {
         _deleteList[id] = softRemove;
       }

       return true;
     }, "Registering remove from UoW", id.ValueAsString);
    }

    public Task<UoWRegisterResult> CleanChanges()
    {
      _dirtyList.Clear();
      _deleteList.Clear();
      _insertList.Clear();

      return Task.FromResult(UoWRegisterResult.Successfull);
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
