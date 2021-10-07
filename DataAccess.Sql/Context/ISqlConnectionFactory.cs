using System.Data;

namespace DataAccess.Sql.Context
{
  internal interface ISqlConnectionFactory
  {
    IDbConnection CreateSqlDbConnection();
  }
}