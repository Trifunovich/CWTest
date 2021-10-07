using System;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Sql.Context
{
  internal class EfContextFactory : IEfContextFactory
  {
    private readonly SqlEfContext _dbContext;

    private static bool _mirated = false;

    public EfContextFactory(SqlEfContext dbContext)
    {
      _dbContext = dbContext;

      if (!_mirated)
      {
        dbContext.Database.Migrate();
        _dbContext.Database.AutoTransactionsEnabled = true;
        _mirated = true;
      }
    }

    public SqlEfContext CreateEfContext()
    {
      return _dbContext;
    }

    public void Dispose()
    {
      _dbContext?.Dispose();
    }
  }
}