using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DataAccess.Sql.Context;
using DataAccess.Sql.Models;

namespace DataAccess.Sql.Repositories
{
  internal class DapperResolver<T> : IDapperResolver<T> where T : SqlDataModelBase
  {
    public IDbConnection Connection { get; }
    protected DbTransaction Transaction;

    public DapperResolver(ISqlConnectionFactory connectionFactory)
    {
      Connection = connectionFactory.CreateSqlDbConnection();
      Connection.Open();
      Transaction = Connection.BeginTransaction() as DbTransaction;
    }
    
    public async Task<int> ExecuteQuery<P>(string query, P param)
    {
      return await Connection.ExecuteAsync(sql: query, param: param, transaction:Transaction);
    }

    public async Task<List<T>> GetResults<P>(string query, P param)
    {
      return (await Connection.QueryAsync<T>(sql:query, param: param, transaction: Transaction)).ToList();
    }

    public async Task<long> Insert(IList<T> records)
    {
      return (await Connection.InsertAsync(records, Transaction));
    }

    public async Task<int> InsertOne(T record)
    {
      return (await Connection.InsertAsync(record, Transaction));
    }

    public async Task<bool> Update(T record)
    {
      return await Connection.UpdateAsync(record, Transaction);
    }


    public DbTransaction SetTransactionAndOpenConnection()
    {
      return Transaction;
    }

    public void CloseEverything()
    {
      Connection?.Close();
      Transaction?.Dispose();
      Connection?.Dispose();
    }
  }
}