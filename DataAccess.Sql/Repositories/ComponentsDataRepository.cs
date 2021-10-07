using DataAccess.Core.Validation;
using DataAccess.Sql.Context;
using DataAccess.Sql.Models;
using LoggingLibrary;

namespace DataAccess.Sql.Repositories
{
  internal class ComponentsDataRepository : EfDataRepository<ComponentModel>
  {
    public ComponentsDataRepository(IEfContextFactory contextFactory, IBasicLogger<ComponentsDataRepository> logger, IRepositoryInputValidator inputValidator) : base(contextFactory, logger, inputValidator)
    {
    }
  }
}