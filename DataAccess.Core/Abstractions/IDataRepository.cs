using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CWTest.Core.DataManipulation;
using LoggingLibrary;

namespace DataAccess.Core.Abstractions
{
    public interface IDbRepo : IDisposable
    {
    }

    public interface IDatabaseRepository<T> : ILoadingDataRepository<T>, IStoringDataRepository<T>
        where T : class, IDataModel
    {

    }


    public interface ILoadingDataRepository<T> : IDbRepo where T : IDataModel
    {
        Task<T> GetById(IDAbstraction id, bool? isActive = true);

        Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive = true);

        Task<IEnumerable<T>> GetAll(Predicate<T> filter);

        Task<IEnumerable<T>> GetAll(bool? isActive = true);

        Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true);

        Task<T> GetFirst(Predicate<T> filter);

        Task<T> GetByLabel(string label, bool? isActive = true);

    }

    public interface IStoringDataRepository<T> : IDbRepo where T : IDataModel
    {
        Task<int?> DeleteById(IDAbstraction id);

        Task<int?> HardDeleteById(IDAbstraction id);

        Task<int?> Insert(IEnumerable<T> records);

        Task<int?> Add(T record);

        Task<int?> DeleteByValue(T record);

        Task<int?> HardDeleteByValue(T record);

        Task<int?> UpdateRecord(T record);

        Task<int?> SaveChanges();

        Task<int?> Apply();

        void SetCancellationToken(CancellationTokenSource cancellationTokenSource);

        Task<int?> RevertAll();
    }

}
