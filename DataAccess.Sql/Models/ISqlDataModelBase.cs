using System;
using DataAccess.Core.Abstractions;

namespace DataAccess.Sql.Models
{
  public interface ISqlDataModelBase : IDataModelBase<Guid>
  {
  }
}