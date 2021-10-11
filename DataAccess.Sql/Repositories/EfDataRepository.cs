using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Validation;
using DataAccess.Sql.Context;
using DataAccess.Sql.Models;
using LoggingLibrary;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Sql.Repositories
{
    internal interface IEfRepo<T> where T : class, IDataModel
    {
        SqlEfContext DbContext { get; }
        DbSet<T> DataSet { get; }
        void SetEfContext(SqlEfContext context);
    }

    internal interface IStoringEfDataRepository<T> : IStoringDataRepository<T>, IEfRepo<T>
        where T : class, IDataModel
    {

    }

    internal interface ILoadingEfDataRepository<T> : ILoadingDataRepository<T>, IEfRepo<T>
        where T : class, IDataModel
    {

    }

    internal abstract class StoringEfDataRepository<T> : StoringDataRepositoryBase<T>, IStoringEfDataRepository<T> where T : class, IDataModel
    {
        public SqlEfContext DbContext { get; protected set; }

        public DbSet<T> DataSet => DbContext.Set<T>();
        public void SetEfContext(SqlEfContext context)
        {
            DbContext = context;
        }
        
        protected StoringEfDataRepository(IRepositoryInputValidator inputValidator, IBasicLoggerAbstract logger) : base(inputValidator, logger)
        {
            Transaction = null;
            LogStartedRepo();
        }

        protected override Func<IEnumerable<T>, Task<int?>> InsertFunction =>
            async (records) =>
            {
                var sqlDataModels = records.ToList();
                await DataSet.AddRangeAsync(sqlDataModels, TokenSource.Token);
                return await Task.FromResult(sqlDataModels.Count());
            };

        protected override Func<T, Task<int?>> AddFunction =>
            async (record) =>
            {
                await DataSet.AddAsync(record, TokenSource.Token);
                return 1;
            };

        protected override Func<T, Task<int?>> HardDeleteByValueFunction =>
            async (record) => await HardDeleteById(record.IdAbstraction);

        protected override Func<T, Task<int?>> UpdateFunction =>
            async (record) =>
            {
                DbContext.Entry(record).State = EntityState.Unchanged;
                return await Task.FromResult(1);
            };

        protected override Func<Task<int?>> SaveChangesFunction =>
            async () => await DbContext.SaveChangesAsync(TokenSource.Token);

        protected override Func<IDAbstraction, bool?, Task<T>> GetByIdFunction =>
            async (id, isActive) =>
            {
                GuidAbstraction guidAbs = id as GuidAbstraction;
                return await DataSet.FirstOrDefaultAsync(c => (isActive == null || c.IsActive == isActive.Value) && c.IdAbstraction.Equals(guidAbs.Value), TokenSource.Token);
            };

    }

    abstract class LoadingEfDataRepository<T> : LoadingDataRepositoryBase<T>, ILoadingEfDataRepository<T>
        where T : class, IDataModel
    {
        public SqlEfContext DbContext { get; protected set; }

        public DbSet<T> DataSet => DbContext.Set<T>();

        protected LoadingEfDataRepository(IRepositoryInputValidator inputValidator, IBasicLoggerAbstract logger) : base(inputValidator, logger)
        {
            Transaction = null;
            LogStartedRepo();
        }

        public void SetEfContext(SqlEfContext context)
        {
            DbContext = context;
        }

        protected override Func<bool?, Task<IList<T>>> GetAllFunction =>
            async (bool? isActive) =>
            {
                return await DataSet.Where(c => isActive == null || c.IsActive == isActive.Value)
                    .ToListAsync(TokenSource.Token);
            };

        protected override Func<DateTime, DateTime?, bool?, Task<IList<T>>> GetAllFunctionWithDates =>
            async (DateTime createdAfter, DateTime? createdBefore, bool? isActive) =>
            {
                return await DataSet.Where(c =>
                        (isActive == null || c.IsActive == isActive.Value) &&
                        c.CreatedOn >= createdAfter &&
                        c.CreatedOn <= (createdBefore ?? DateTime.Now))
                    .ToListAsync(TokenSource.Token);
            };

        protected override Func<Predicate<T>, Task<IList<T>>> GetAllWithFilterFunction =>
            async (crit) =>
            {
                return await DataSet.Where(c => crit(c))
                    .ToListAsync(TokenSource.Token);
            };

        protected override Func<string, bool?, Task<T>> GetByLabelFunction =>
            async (label, isActive) =>
            {
                return await DataSet.FirstOrDefaultAsync(
                    c => (isActive == null || c.IsActive == isActive.Value) &&
                         label.Equals(c.Label, StringComparison.InvariantCultureIgnoreCase), TokenSource.Token);
            };

        protected override Func<Predicate<T>, Task<T>> GetFirstAfterFilterFunction =>
            async (crit) => { return await DataSet.FirstOrDefaultAsync(c => crit(c), TokenSource.Token); };

        protected override Func<PagingParameters, bool?, Task<IList<T>>> GetPageFunction =>
            async (PagingParameters pager, bool? isActive) =>
            {
                var results = await DataSet.Where(x => isActive == null || x.IsActive == isActive)
                    .Skip(pager.FirstElementPosition).Take(pager.PageSize).ToListAsync(TokenSource.Token);
                return results;
            };

        protected override Func<IDAbstraction, bool?, Task<T>> GetByIdFunction =>
            async (id, isActive) =>
            {
                GuidAbstraction guidAbs = id as GuidAbstraction;
                //todo: make equality comparer for IDAbstractions
                return await DataSet.FirstOrDefaultAsync(c => (isActive == null || c.IsActive == isActive.Value) && c.IdAbstraction.Equals(guidAbs.Value), TokenSource.Token);
            };
    }

    internal abstract class DatabaseRepositoryAdapter<T> : IDatabaseRepository<T> where T : class, IDataModel
    {
        private readonly ILoadingDataRepository<T> _loadingRepo;
        private readonly IStoringDataRepository<T> _storingRepo;

        protected DatabaseRepositoryAdapter(IEfContextFactory efContextFactory, ILoadingEfDataRepository<T> loadingRepo, IStoringEfDataRepository<T> storingRepo)
        {
            var context = efContextFactory.CreateEfContext();
            loadingRepo.SetEfContext(context);
            storingRepo.SetEfContext(context);
            
            _loadingRepo = loadingRepo;
            _storingRepo = storingRepo;
        }

        public void Dispose()
        {
            _loadingRepo.Dispose();
            _storingRepo.Dispose();
        }
        public Task<T> GetById(IDAbstraction id, bool? isActive)
        {
            return _loadingRepo.GetById(id, isActive);
        }

        public Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive)
        {
            return _loadingRepo.GetPage(pagingParameters, isActive);
        }

        public Task<IEnumerable<T>> GetAll(Predicate<T> filter)
        {
            return _loadingRepo.GetAll(filter);
        }

        public Task<IEnumerable<T>> GetAll(bool? isActive)
        {
            return _loadingRepo.GetAll(isActive);
        }

        public Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true)
        {
            return _loadingRepo.GetAll(createdAfter, createdBefore, isActive);
        }

        public Task<T> GetFirst(Predicate<T> filter)
        {
            return _loadingRepo.GetFirst(filter);
        }

        public Task<T> GetByLabel(string label, bool? isActive)
        {
            return _loadingRepo.GetByLabel(label, isActive);
        }

        public Task<int?> DeleteById(IDAbstraction id)
        {
            return _storingRepo.DeleteById(id);
        }

        public Task<int?> HardDeleteById(IDAbstraction id)
        {
            return _storingRepo.HardDeleteById(id);
        }

        public Task<int?> Insert(IEnumerable<T> records)
        {
            return _storingRepo.Insert(records);
        }

        public Task<int?> Add(T record)
        {
            return _storingRepo.Add(record);
        }

        public Task<int?> DeleteByValue(T record)
        {
            return _storingRepo.DeleteByValue(record);
        }

        public Task<int?> HardDeleteByValue(T record)
        {
            return _storingRepo.HardDeleteByValue(record);
        }

        public Task<int?> UpdateRecord(T record)
        {
            return _storingRepo.UpdateRecord(record);
        }

        public Task<int?> SaveChanges()
        {
            return _storingRepo.SaveChanges();
        }

        public Task<int?> Apply()
        {
            return _storingRepo.Apply();
        }

        public void SetCancellationToken(CancellationTokenSource cancellationTokenSource)
        {
            _storingRepo.SetCancellationToken(cancellationTokenSource);
        }

        public Task<int?> RevertAll()
        {
            return _storingRepo.RevertAll();
        }
    }
}