using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Extensions;
using DataAccess.Core.Validation;
using DataAccess.Sql.Models;
using LoggingLibrary;

namespace DataAccess.Sql.Repositories
{
    internal class DapperDataRepository<T> : DataRepositoryBase<T> where T : ISqlDataModelBase
    {
        protected readonly IDapperResolver<T> Resolver;

        protected string TableName = typeof(T)
          ?.GetCustomAttributesFromClass<System.ComponentModel.DataAnnotations.Schema.TableAttribute>(false)
          .FirstOrDefault()?.Name;

        protected string GetActiveCheckString(bool? isActive, string activePrefix, bool whereNeeded)
        {
            string beginning = whereNeeded ? "where" : "and";
            return isActive == null ? string.Empty : $" {beginning} {activePrefix}{ nameof(SqlDataModelBase.IsActive) } = @IsActive";
        }

        public DapperDataRepository(IDapperResolver<T> resolver, IBasicLogger<DapperDataRepository<T>> logger, IRepositoryInputValidator inputValidator) : base(logger, inputValidator)
        {
            Resolver = resolver;

            LogStartedRepo();
        }

        protected override Func<PagingParameters, bool?, Task<IList<T>>> GetPageFunction =>
          async (PagingParameters pager, bool? isActive) =>
          {
              string activeCheck = GetActiveCheckString(isActive, string.Empty, true);
              var sql = ($@" select * from [dbo].[{TableName}]{activeCheck}
                       ORDER BY Id
                       OFFSET      @FirstElementPosition ROWS 
                       FETCH NEXT  @PageSize   ROWS ONLY");
              var results = await Resolver.GetResults(sql.Trim(), new DapperPagingParams(pager, isActive));
              return results;
          };


        protected override Func<bool?, Task<IList<T>>> GetAllFunction =>
         async (bool? isActive) =>
         {
             string activeCheck = GetActiveCheckString(isActive, string.Empty, true);
             var sql = ($@" select * from [dbo].[{TableName}]{activeCheck}
                       ORDER BY Id");
             var results = await Resolver.GetResults<dynamic>(sql.Trim(), null);
             return results;
         };

        protected override Func<Predicate<T>, Task<IList<T>>> GetAllWithFilterFunction => null;
        protected override Func<Predicate<T>, Task<T>> GetFirstAfterFilterFunction => null;

        protected override Func<DateTime, DateTime?, bool?, Task<IList<T>>> GetAllFunctionWithDates =>
        async (DateTime createdAfter, DateTime? createdBefore, bool? isActive) =>
          {
              var param = new { CreatedAfter = createdAfter, CreatedBefore = createdBefore ?? DateTime.Now, IsActive = isActive };
              string activeCheck = GetActiveCheckString(isActive, string.Empty, false);

              var sql = ($@" select * from [dbo].[{TableName}]
                       ORDER BY Id
                       Where {nameof(SqlDataModelBase.CreatedOn)} >= @{nameof(param.CreatedAfter)} and {nameof(SqlDataModelBase.CreatedOn)} <= @{nameof(param.CreatedBefore)}{activeCheck}");

              var results = await Resolver.GetResults<dynamic>(sql, param);
              return results;
          };

        protected override Func<string, bool?, Task<T>> GetByLabelFunction =>
          async (label, isActive) =>
          {
              var param = new { Label = label, IsActive = isActive };
              string activeCheck = GetActiveCheckString(isActive, string.Empty, true);

              var sql = ($@" select * from [dbo].[{TableName}]{activeCheck}
                       ORDER BY Id");

              var results = (await Resolver.GetResults<dynamic>(sql, param)).FirstOrDefault();
              return results;
          };

        protected override Func<IEnumerable<T>, Task<int?>> GetInsertFunction =>
          async (records) =>
          {
              var dataModels = records.ToList();
              var results = (await Resolver.Insert(dataModels));
              return results > int.MaxValue ? int.MaxValue : (int)results;
          };

        protected override Func<T, Task<int?>> GetAddFunction =>
          async (record) =>
          {
              var results = (await Resolver.InsertOne(record));
              return results;
          };

        protected override Func<T, Task<int?>> GetHardDeleteByValueFunction =>
          async (record) => await HardDeleteById(record.IdAbstraction);

        protected override Func<T, Task<int?>> GetUpdateFunction =>
          async (record) =>
          {
              var results = (await Resolver.Update(record));
              return results ? 1 : 0;
          };

        protected override Func<Task<int?>> GetSaveChangesFunction =>
          async () => await Task.FromResult(1);



        protected override Func<IDAbstraction, bool?, Task<T>> GetByIdFunction =>
          async (id, isActive) =>
          {
              GuidAbstraction guidAbs = id as GuidAbstraction;
              T results = default(T);

              if (guidAbs != null)
              {
                  var param = new { Id = guidAbs.Value, IsActive = isActive };
                  string activeCheck = GetActiveCheckString(isActive, string.Empty, true);

                  var sql = ($@" select * from [dbo].[{TableName}]{activeCheck} and Id = @Id");

                  results = (await Resolver.GetResults<dynamic>(sql, param)).FirstOrDefault(r => r.IsActive);
              }

              return results;
          };

        public override void Dispose()
        {
            base.Dispose();
            LogDisposedRepo();
        }

        protected class DapperPagingParams : PagingParameters
        {

            public bool? IsActive { get; }

            public DapperPagingParams(int page, int pageSize = 10, int offset = 0, bool? isActive = true) : base(page, pageSize, offset)
            {
                IsActive = isActive;
            }

            public DapperPagingParams(PagingParameters pagingParameters, bool? isActive) : this(pagingParameters.Page,
              pagingParameters.PageSize, pagingParameters.Offset, isActive)
            {

            }

        }
    }



}