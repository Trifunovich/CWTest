using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Validation;
using DataAccess.Excel.Context;
using DataAccess.Excel.ExcelManipulation;
using DataAccess.Excel.Models;
using LoggingLibrary;

namespace DataAccess.Excel.Repositories
{
    abstract class ExcelDataRepository<T> : LoadingDataRepositoryBase<T> where T : class, IDataModel
    {
        private readonly IExcelContext _context;
        private readonly IExtractor _extractor;

        protected ExcelDataRepository(IExcelContext context, IExtractor extractor, IBasicLoggerAbstract logger, IRepositoryInputValidator inputValidator) : base(inputValidator, logger)
        {
            _context = context;
            _extractor = extractor;
            LogStartedRepo();
        }

        private async Task<IList<T>> GetAllOfThem(int i)
        {
            var imported = await _extractor.Import(_context.ExcelPath, CancellationToken.None);

            if (i > 1)
            {
                return imported.ElementAt(i - 1).Value.OfType<T>().ToList();
            }
            else
            {
                List<T> newList = new List<T>();

                foreach (var keyValuePair in imported)
                {
                    newList.AddRange(keyValuePair.Value.OfType<T>());
                }

                return newList;
            }
         
        }

        protected override Func<bool?, Task<IList<T>>> GetAllFunction =>
            async (bool? isActive) => await GetAllOfThem(-1);

        protected override Func<DateTime, DateTime?, bool?, Task<IList<T>>> GetAllFunctionWithDates =>
          async (DateTime createdAfter, DateTime? createdBefore, bool? isActive) =>
              await GetAllOfThem(-1);

        protected override Func<Predicate<T>, Task<IList<T>>> GetAllWithFilterFunction =>
            async (crit) =>
                await GetAllOfThem(-1);

        protected override Func<string, bool?, Task<T>> GetByLabelFunction =>
          async (label, isActive) =>
              null;

        protected override Func<Predicate<T>, Task<T>> GetFirstAfterFilterFunction =>
            async (crit) =>
                null;
        
        protected override Func<PagingParameters, bool?, Task<IList<T>>> GetPageFunction =>
          async (PagingParameters pager, bool? isActive) =>
              await GetAllOfThem(pager.Page);


        protected override Func<IDAbstraction, bool?, Task<T>> GetByIdFunction =>
          async (id, isActive) =>
              null;
        }
}