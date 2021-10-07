using System;
using System.Collections.Generic;
using System.Linq;
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
  abstract class EfDataRepository<T> : DataRepositoryBase<T> where T : class, ISqlDataModelBase
  {
    protected readonly SqlEfContext DbContext;

    protected EfDataRepository(IEfContextFactory contextFactory, IBasicLoggerAbstract logger, IRepositoryInputValidator inputValidator) : base(logger, inputValidator)
    {
      DbContext = contextFactory.CreateEfContext();
      Transaction = null;
      LogStartedRepo();
    }

    protected DbSet<T> DataSet => DbContext.Set<T>();

    protected override Func<bool?, Task<IList<T>>> GetAllFunction =>
      async (bool? isActive) =>
      {
        return await DataSet.Where(c => isActive == null || c.IsActive == isActive.Value).ToListAsync(TokenSource.Token);
      };
    
    protected override Func<DateTime, DateTime?, bool?, Task<IList<T>>> GetAllFunctionWithDates =>
      async(DateTime createdAfter, DateTime? createdBefore, bool? isActive) =>
      {
        return await DataSet.Where(c =>
            (isActive == null || c.IsActive == isActive.Value) &&
            c.CreatedOn >= createdAfter &&
            c.CreatedOn <= (createdBefore ?? DateTime.Now))
            .ToListAsync(TokenSource.Token);
      };

    protected override Func<string, bool?, Task<T>> GetByLabelFunction =>
      async (label, isActive) =>
      {
        return await DataSet.FirstOrDefaultAsync(c => (isActive == null || c.IsActive == isActive.Value) && label.Equals(c.Label, StringComparison.InvariantCultureIgnoreCase), TokenSource.Token);
      };

    protected override Func<IEnumerable<T>, Task<int?>> GetInsertFunction =>
      async (records) =>
      {
        var sqlDataModels = records.ToList();
        await DataSet.AddRangeAsync(sqlDataModels, TokenSource.Token);
        return await Task.FromResult(sqlDataModels.Count());
      };

    protected override Func<T, Task<int?>> GetAddFunction =>
      async (record  ) =>
      {
        await DataSet.AddAsync(record, TokenSource.Token);
        return 1;
      };

    protected override Func<T, Task<int?>> GetHardDeleteByValueFunction =>
      async (record) => await HardDeleteById(record.IdAbstraction);

    protected override Func<T, Task<int?>> GetUpdateFunction =>
      async (record) =>
      {
        DbContext.Entry(record).State = EntityState.Unchanged;
        return await Task.FromResult(1);
      };

    protected override Func<Task<int?>> GetSaveChangesFunction =>
      async () => await DbContext.SaveChangesAsync(TokenSource.Token);

    protected override Func<PagingParameters, bool?, Task<IList<T>>> GetPageFunction =>
      async (PagingParameters pager, bool? isActive) =>
      {
        var results = await DataSet.Where(x => isActive == null || x.IsActive == isActive).Skip(pager.FirstElementPosition).Take(pager.PageSize).ToListAsync(TokenSource.Token);
        return results;
      };


    protected override Func<IDAbstraction, bool?, Task<T>> GetByIdFunction =>
      async (id, isActive) =>
      {
        GuidAbstraction guidAbs = id as GuidAbstraction;
        return await DataSet.FirstOrDefaultAsync(c => (isActive == null || c.IsActive == isActive.Value) && c.Id.Equals(guidAbs.Value), TokenSource.Token);
      };
    
    
    public override void Dispose()
    {
      base.Dispose();
      //DbContext?.Dispose();
      LogDisposedRepo();
    }
  }
}