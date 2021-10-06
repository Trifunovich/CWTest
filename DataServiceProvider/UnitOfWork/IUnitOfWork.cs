using DataServiceProvider.Core.DtoAbstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataServiceProvider.UnitOfWork
{
  /// <summary>
  /// Classic unit of work pattern, it takes a repo and aggregates the changes, waiting for the commit
  /// </summary>
  public interface IUnitOfWork<T, TId> where T : IDto<TId>
  {
    /// <summary>
    /// The item needs to be updated inside of the database
    /// </summary>
    /// <param name="item">Database entity</param>
    bool RegisterDirty(T item, IdAbstraction<TId> id);

    /// <summary>
    /// The item doesn't need updating, removing it from clean registry
    /// </summary>
    /// <param name="item">Database entity</param>
    bool RegisterClean(T item);

    /// <summary>
    /// The item needs to be removed from the database
    /// </summary>
    /// <param name="id">Item id - GUID</param>
    /// <param name="softRemove">Should it be marked as inactive or deleted totally</param>
    bool RegisterRemove(IdAbstraction<TId> id, bool softRemove);

    /// <summary>
    /// Inserts the collection of data
    /// </summary>
    /// <param name="records">The list of database entities</param>
    bool RegisterInsert(IEnumerable<T> records);

    /// <summary>
    /// Gets a page by parametrs
    /// </summary>
    /// <param name="pagingParameters">Paging params <see cref="PagingParameters"/></param>
    /// <param name="isActive">Is it active in the database or is it marked as inactive, null - get everything</param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, bool? isActive = true);

    /// <summary>
    /// Gets a filtered page by parameters
    /// </summary>
    /// <param name="pagingParameters">Paging params <see cref="PagingParameters"/></param>
    /// <param name="filter">Filter for finding the right entities/></param>
    /// <param name="isActive">Is it active in the database or is it marked as inactive, null - get everything</param>
    Task<IEnumerable<T>> GetPage(PagingParameters pagingParameters, Predicate<T> filter);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns>A list of dtos</returns>
    Task<IEnumerable<T>> GetAll(bool? isActive = true);

    /// <summary>
    /// Gets all entities between two dates and whether they are active
    /// </summary>
    /// <param name="createdAfter">From which date</param>
    /// <param name="createdBefore">To which date, null - no upper limit</param>
    /// <param name="isActive">Is it active in the database or is it marked as inactive, null - get everything</param>
    /// <returns>A list of dtos</returns>
    Task<IEnumerable<T>> GetAll(DateTime createdAfter, DateTime? createdBefore = null, bool? isActive = true);

    /// <summary>
    /// Gets all filered-out entities
    /// </summary>
    /// <param name="filter">Filter for finding the right entities</param>
    /// <returns>A list of dtos</returns>
    Task<IEnumerable<T>> GetAll(Predicate<T> filter);


    /// <summary>
    /// Gets the first entity by a certain criteria
    /// </summary>
    /// <param name="filter">Filter for finding the right entity</param>
    /// <returns>A dto</returns>
    Task<T> GetFirst(Predicate<T> filter);

    /// <summary>
    /// Commits everything in the cache
    /// </summary>
    /// <returns>Whether all of the operations were successful, if not it returns the internal error description</returns>
    /// <remarks>YNot needed for data loading</remarks>
    Task<UoWAggregatedResult> SaveChanges();

    /// <summary>
    /// Cancels the transaction in progress
    /// </summary>
    /// <returns>Whether all of the operations were successful, if not it returns the internal error description</returns>
    Task<UoWAggregatedResult> RevertAll();

    /// <summary>
    /// Cleans the caches
    /// </summary>
    /// <returns>Whether all of the operations were successful</returns>
    Task<UoWRegisterResult> CleanChanges();
  }
}
