using Autofac;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Validation;
using DataAccess.CWTest.Abstraction;
using DataAccess.Excel.Context;
using DataAccess.Excel.ExcelManipulation;
using DataAccess.Excel.Models;
using LoggingLibrary;

namespace DataAccess.Excel.Repositories
{
  internal class ComponentsDataRepository : ExcelDataRepository<IComponentSpecification>
  {
      public ComponentsDataRepository(IExcelContext context, IExtractor extractor, IBasicLogger<ComponentsDataRepository> logger, IRepositoryInputValidator inputValidator) : base(context, extractor, logger, inputValidator)
      {
      }
  }
}