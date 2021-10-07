using System;
using System.IO;
using System.Reflection;
using DataAccess.Sql.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Sql.Context
{
  /// <summary>
  /// This is needed for package manager console
  /// </summary>
  internal class SqlEfContextFactory : IDesignTimeDbContextFactory<SqlEfContext>
  {
    SqlEfContext IDesignTimeDbContextFactory<SqlEfContext>.CreateDbContext(string[] args)
    {
      var builder = new DbContextOptionsBuilder<SqlEfContext>();
      string path = VisualStudioProvider.TryGetSolutionDirectoryInfo().FullName + @"\DataAccess.Sql\LocalSqliteDb.db";
   
      File.WriteAllText(@"..\Logs\DbUpdatingPath.txt", path);

      builder.UseSqlServer($@"Data Source=LocalSqliteDb.db");
      return new SqlEfContext(builder.Options, null);
    }
  }
}