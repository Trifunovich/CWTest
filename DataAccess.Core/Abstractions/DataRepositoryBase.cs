
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CWTest.Core.DataManipulation;
using DataAccess.Core.Validation;
using LoggingLibrary;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DataAccess.Core.Abstractions
{
    public abstract class DbRepoBase<T> where T : IDataModel
    {
        protected IBasicLoggerAbstract Logger;
        protected readonly IRepositoryInputValidator InputValidator;
        protected DbTransaction Transaction;
        protected CancellationTokenSource TokenSource = new CancellationTokenSource();

        protected DbRepoBase(IRepositoryInputValidator inputValidator, IBasicLoggerAbstract logger)
        {
            InputValidator = inputValidator;
            Logger = logger;
        }

        protected abstract Func<IDAbstraction, bool?, Task<T>> GetByIdFunction { get; }

        public virtual async Task<T> GetById(IDAbstraction id, bool? isActive = true)
        {
            if (InputValidator.ValidateGenericId(id))
            {
                var res = await GetByIdFunction.Invoke(id, isActive);

                if (InputValidator.ValidateValue(res))
                {
                    LogGotOneFromDatabase(res);
                    return res;
                }

                return default;
            }

            LogNoId(id);
            return default; ;
        }

        protected void LogGotOneFromDatabase(T one)
        {
            Logger?.LogInformation("Got {one} from the repo", one);
        }
        protected void LogNoId(object id)
        {
            Logger?.LogError("IdAbstraction with value {id} not found for type {typename}", id, typeof(T).Name);
        }
        protected void LogStartedRepo()
        {
            Logger.LogInformation("Repo created for {name}", typeof(T).Name);
        }
        protected void LogDisposedRepo()
        {
            Logger?.LogInformation("Disposed {name} repo", typeof(T).Name);
        }

        public virtual void Dispose()
        {
            Transaction?.Dispose();
            Transaction = null;
            TokenSource?.Dispose();
            TokenSource = null;
            Logger?.LogInformation("Resources in data repository base of type {name} disposed", typeof(T).Name);
        }
    }

    public abstract class LoadingDataRepositoryBase<T> : DbRepoBase<T>, ILoadingDataRepository<T> where T : IDataModel
    {
        protected LoadingDataRepositoryBase(IRepositoryInputValidator inputValidator, IBasicLoggerAbstract logger) : base(inputValidator, logger)
        {
        }

        protected abstract Func<bool?, Task<IList<T>>> GetAllFunction { get; }
        protected abstract Func<Predicate<T>, Task<IList<T>>> GetAllWithFilterFunction { get; }
        protected abstract Func<Predicate<T>, Task<T>> GetFirstAfterFilterFunction { get; }
        protected abstract Func<DateTime, DateTime?, bool?, Task<IList<T>>> GetAllFunctionWithDates { get; }
        protected abstract Func<string, bool?, Task<T>> GetByLabelFunction { get; }
        protected abstract Func<PagingParameters, bool?, Task<IList<T>>> GetPageFunction { get; }
        
        public virtual async Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive = true)
        {
            if (InputValidator.ValidatePagingParams(pagingParameters))
            {
                var res = await GetPageFunction.Invoke(pagingParameters, isActive);

                if (InputValidator.ValidateValue(res))
                {
                    LogGotAll(res.Count);
                    return res;
                }
            }

            return default;
        }

        public virtual async Task<IEnumerable<T>> GetAll(bool? isActive = true)
        {
            var res = await GetAllFunction.Invoke(isActive);

            if (InputValidator.ValidateValue(res))
            {
                LogGotAll(res.Count);
                return res;
            }

            return default;
        }
        public virtual async Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true)
        {
            bool areParamsValid = InputValidator.ValidateGetAllParams(createdAfter, createdBefore);

            if (areParamsValid)
            {
                var res = await GetAllFunctionWithDates.Invoke(createdAfter, createdBefore, isActive);
                LogGotAll(res.Count);
                return res;
            }

            return default;
        }

        public virtual async Task<IEnumerable<T>> GetAll(Predicate<T> filter)
        {
            var res = await GetAllWithFilterFunction.Invoke(filter);
            LogGotAll(res.Count);
            return res;
        }

        public virtual async Task<T> GetFirst(Predicate<T> filter)
        {
            var res = await GetFirstAfterFilterFunction.Invoke(filter);

            if (InputValidator.ValidateValue(res))
            {
                LogGotOneFromDatabase(res);
                return res;
            }

            return default;
        }


        public virtual async Task<T> GetByLabel(string label, bool? isActive = true)
        {
            if (InputValidator.ValidateLabel(label))
            {
                var res = await GetByLabelFunction.Invoke(label, isActive);

                if (InputValidator.ValidateValue(res))
                {
                    LogGotOneFromDatabase(res);
                    return res;
                }

                return default;
            }

            return default;
        }

        protected void LogGotAll(int count)
        {
            Logger?.LogInformation("Got {count} of {TypeName} from the repo", count, typeof(T).Name);
        }
    }

    public abstract class StoringDataRepositoryBase<T> : DbRepoBase<T> , IStoringDataRepository<T> where T : IDataModel
    {

        protected StoringDataRepositoryBase(IRepositoryInputValidator inputValidator, IBasicLoggerAbstract logger) : base(inputValidator, logger)
        {
        }

        protected abstract Func<IEnumerable<T>, Task<int?>> InsertFunction { get; }
        protected abstract Func<T, Task<int?>> AddFunction { get; }
        protected abstract Func<T, Task<int?>> HardDeleteByValueFunction { get; }
        protected abstract Func<T, Task<int?>> UpdateFunction { get; }
        protected abstract Func<Task<int?>> SaveChangesFunction { get; }

        private async void OnCancel()
        {
            Logger?.LogInformation("Cancellation requested");
            await Rollback();
        }

        private async Task<int?> DeleteByIdImpl(IDAbstraction id, bool isHard)
        {
            if (InputValidator.ValidateGenericId(id))
            {
                T found = await GetById(id, null);
                return await DeleteByValueImpl(found, isHard);
            }

            LogNoId(id);
            return default;
        }

        public virtual async Task<int?> DeleteById(IDAbstraction id)
        {
            return await DeleteByIdImpl(id, false);
        }

        public virtual async Task<int?> HardDeleteById(IDAbstraction id)
        {
            return await DeleteByIdImpl(id, true);
        }


        public virtual async Task<int?> Insert(IEnumerable<T> records)
        {
            var sqlDataModels = records.ToList();
            if (InputValidator.ValidateInputList(sqlDataModels))
            {
                await InsertFunction(sqlDataModels);
                LogInsertedIntoDb(sqlDataModels.Count());
                return sqlDataModels.Count();
            }

            return null; ;
        }

        public virtual async Task<int?> Add(T record)
        {
            if (InputValidator.ValidateValue(record))
            {
                var res = await AddFunction(record);
                LogAddedRecordToDb(record);
                return res;
            }

            return null;
        }

        public async Task<int?> DeleteByValue(T record)
        {
            return await DeleteByValueImpl(record, false);
        }

        protected async Task<int?> DeleteByValueImpl(T record, bool isHard)
        {
            if (InputValidator.ValidateValue(record))
            {
                if (isHard)
                {
                    var res = HardDeleteByValueFunction(record);
                }
                else
                {
                    record.IsActive = false;
                }

                LogDeletedRecordFromDb(record, isHard);
                return await Task.FromResult(1);
            }

            return await Task.FromResult<int?>(default);
        }

        public virtual async Task<int?> HardDeleteByValue(T record)
        {
            return await DeleteByValueImpl(record, true);
        }

        public virtual async Task<int?> UpdateRecord(T record)
        {
            if (InputValidator.ValidateValue(record))
            {
                var res = await UpdateFunction(record);
                LogUpdatedRecordInDb(record);
                return res;
            }

            return default;
        }

        public virtual async Task<int?> SaveChanges()
        {
            await Apply();
            var res = await SaveChangesFunction.Invoke();
            LogSavedChanges();
            return res;
        }

        public virtual Task<int?> Apply()
        {
            Transaction?.Commit();
            Logger?.LogInformation("Transaction committed");
            return Task.FromResult((int?)1);
        }

        public void SetCancellationToken(CancellationTokenSource cancellationTokenSource)
        {
            TokenSource = cancellationTokenSource;
            Logger?.LogInformation("Cancellation token set");
            TokenSource.Token.Register(OnCancel);
        }

        public async Task<int?> RevertAll()
        {
            await Rollback();
            return 1;
        }

        protected virtual async Task Rollback()
        {
            //await Transaction.RollbackAsync();

            Logger?.LogInformation("Transaction rolled back");
            await Task.CompletedTask;
        }

        protected void LogInsertedIntoDb(int count)
        {
            Logger?.LogInformation("Inserted {count} to the repo", count);
        }

        protected void LogAddedRecordToDb(T one)
        {
            Logger?.LogInformation("Added {one} to the repo", one);
        }

        protected void LogDeletedRecordFromDb(T one, bool isHard)
        {
            Logger?.LogInformation("{HardType} deleted {one} from the repo", GetHardString(isHard), one);
        }

        private string GetHardString(bool isHard)
        {
            return isHard ? "Hard" : "Soft";
        }

        protected void LogUpdatedRecordInDb(T one)
        {
            Logger?.LogInformation("Updated {one} in repo", one);
        }

        protected void LogSavedChanges()
        {
            Logger?.LogInformation("Saved changes for {name} repo", typeof(T).Name);
        }
    }
}