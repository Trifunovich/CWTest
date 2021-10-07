using System;

namespace DataAccess.Sql.Context
{
  internal interface IEfContextFactory : IDisposable
  {
    SqlEfContext CreateEfContext();
  }
}