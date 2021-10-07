using System;
using System.Data;
using System.IO;
using DataAccess.Sql.Helpers;

namespace DataAccess.Sql.Context
{
  internal class SqlConnectionFactory : ISqlConnectionFactory
  {
    public IDbConnection CreateSqlDbConnection()
    {
      string basePath = Directory.GetCurrentDirectory();
      string connString = ConnectionHelper.SqlConnectionString;
      // Connection string setting
     // var connectionStringValue = $@"Data Source={Path.Combine(basePath, connString)}";

      var connectionStringValue = connString;

      // Assume failure.
      IDbConnection connection;

      // Null connection string cannot be accepted
      if (connectionStringValue == null) return null;

      // Create the DbProviderFactory and DbConnection.
      try
      {
        // Create Connection
        connection = new System.Data.SqlClient.SqlConnection(connectionStringValue) {ConnectionString = connectionStringValue};
      }
      catch (Exception ex)
      {
        connection = null;
      }

      // Return the connection.
      return connection;
    }
  }
}

