using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using DataServiceProvider.Core.DtoAbstraction;
using LoggingLibrary;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace DataServiceProvider.Core.UnitOfWork
{
    public abstract class UnitOfWorkBase<T, U> : IUnitOfWork<T>
        where T : IDto where U : IDataModel
    {
        private readonly IList<T> _dirtyList = new List<T>();
        private readonly List<T> _insertList = new List<T>();
        private readonly ConcurrentDictionary<IDAbstraction, bool> _deleteList = new ConcurrentDictionary<IDAbstraction, bool>();

        private readonly IBasicLoggerAbstract _logger;
        private readonly ILoadingDataRepository<U> _loadRepo;
        private readonly IStoringDataRepository<U> _storeRepo;
        private readonly IMapper _mapper;

        protected UnitOfWorkBase(IBasicLoggerAbstract logger, ILoadingDataRepository<U> loadRepo, IStoringDataRepository<U> storeRepo, IMapper mapper)
        {
            _logger = logger;
            _loadRepo = loadRepo;
            _storeRepo = storeRepo;
            _mapper = mapper;
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

        public virtual async Task<IEnumerable<T>> GetAll(bool? isActive = true)
        {
            var rawModels = await _loadRepo.GetAll(isActive);
            return MapList<T, U>(rawModels);
        }

        public virtual async Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true)
        {
            var rawModels = await _loadRepo.GetAll(createdAfter, createdBefore, isActive);
            return MapList<T, U>(rawModels);
        }

        public virtual async Task<IEnumerable<T>> GetAll(int internalId)
        {
            var rawModels = await _loadRepo.GetAll(x => x.InternalId == internalId);
            return MapList<T, U>(rawModels);
        }

        public virtual async Task<T> GetFirst(int internalId)
        {
            return MapOneElement<T, U>(await _loadRepo.GetFirst(x => x.InternalId == internalId));
        }

        public virtual async Task<T> GetFirst(string label)
        {
            return MapOneElement<T, U>(await _loadRepo.GetFirst(x => x.Label.Equals(label, StringComparison.InvariantCultureIgnoreCase)));
        }

        public virtual async Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive = true)
        {
            var rawModels = await _loadRepo.GetPage(pagingParameters, isActive);
            return MapList<T, U>(rawModels);
        }

        public virtual async Task<UoWAggregatedResult> RevertAll()
        {
            var result = await _storeRepo.RevertAll();
            return GetResultBasedOnInt(result, "Failed to revert transactions");
        }

        public virtual async Task<UoWAggregatedResult> SaveChanges()
        {
            bool actuallyDidSomething = false;

            if (_dirtyList.Count > 0)
            {
                actuallyDidSomething = true;
                foreach (var d in _dirtyList)
                {
                    var mapped = MapOneElement<U, T>(d);
                    var updatingSuccess = await _storeRepo.UpdateRecord(mapped);

                    if (updatingSuccess != 1)
                    {
                        return GetResultBasedOnInt(updatingSuccess, "Failed modify element in repo cache");
                    }
                }
            }

            if (_insertList.Count > 0)
            {
                actuallyDidSomething = true;
                var mapped = MapList<U, T>(_insertList);
                var insertingSuccess = await _storeRepo.Insert(mapped);

                if (insertingSuccess != 1)
                {
                    return GetResultBasedOnInt(insertingSuccess, "Failed to insert elements to repo cache");
                }
            }

            if (_deleteList.Count > 0)
            {
                actuallyDidSomething = true;
                foreach (var d in _deleteList)
                {
                    var deleteSuccess = d.Value ? await _storeRepo.HardDeleteById(d.Key) : await _storeRepo.DeleteById(d.Key);

                    if (deleteSuccess != 1)
                    {
                        return GetResultBasedOnInt(deleteSuccess, "Failed to insert elements to repo cache");
                    }
                }
            }

            if (actuallyDidSomething)
            {
                var result = await _storeRepo.SaveChanges();
                return GetResultBasedOnInt(result, "Failed to save data");
            }
            
            return GetResultBasedOnInt(1, string.Empty);
        }

        private UoWAggregatedResult GetResultBasedOnInt(int? i, string onFailedMessage)
        {
            switch (i)
            {
                case 1:
                    {
                        return new UoWAggregatedResult(UoWRegisterResult.Successfull, string.Empty);
                    }
                case null:
                    {
                        return new UoWAggregatedResult(UoWRegisterResult.FailedToConnectToDatabase, "Failed to connect to the database");
                    }
                default:
                    {
                        return new UoWAggregatedResult(UoWRegisterResult.Error, onFailedMessage);
                    }
            }
        }

        private Q MapOneElement<Q, R>(R model)
        {
            return _mapper.Map<Q>(model);
        }

        private IEnumerable<Q> MapList<Q, R>(IEnumerable<R> sourceList)
        {
            return sourceList.Select(MapOneElement<Q, R>);
        }
    }
}
