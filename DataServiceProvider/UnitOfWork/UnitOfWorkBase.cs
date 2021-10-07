using CWTest.Core.DataManipulation;
using DataServiceProvider.Core.DtoAbstraction;
using LoggingLibrary;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceProvider.Core.UnitOfWork
{
  public abstract class UnitOfWorkBase<T> : IUnitOfWork<T> where T : IDto
  {
    private readonly IList<T> _dirtyList = new List<T>();
    private readonly List<T> _insertList = new List<T>();
    private readonly ConcurrentDictionary<IDAbstraction, bool> _deleteList = new ConcurrentDictionary<IDAbstraction, bool>();

    private readonly IBasicLoggerAbstract _logger;

    public UnitOfWorkBase(IBasicLoggerAbstract logger)
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

    public bool RegisterDirty(T item)
    {
      return RegisterAndLog(
       () =>
       {
         if (!_dirtyList.Contains(item))
         {
           _dirtyList.Add(item);
         }

         return true;
       }, "Uow element marked as dirty", item.Id.ValueAsString);     
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

    public bool RegisterRemove(IDAbstraction id, bool softRemove)
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

    public abstract Task<IEnumerable<T>> GetAll(bool? isActive = true);

    public abstract Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true);

    public abstract Task<IEnumerable<T>> GetAll(Predicate<T> filter);

    public abstract Task<T> GetFirst(Predicate<T> filter);

    public abstract Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive = true);

    public abstract Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, Predicate<T> filter);

    public abstract Task<UoWAggregatedResult> RevertAll();

    public abstract Task<UoWAggregatedResult> SaveChanges();
  }
}
