using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using DataAccess.Core.Validation;
using DataAccess.Sql.Context;
using DataAccess.Sql.Models;
using LoggingLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CWTest.Abstraction;

namespace DataAccess.Sql.Repositories
{
    internal class LoadingComponentsDbRepository : LoadingEfDataRepository<IComponentSpecification>
    {
        public LoadingComponentsDbRepository(IRepositoryInputValidator inputValidator, IBasicLogger<LoadingComponentsDbRepository> logger) : base(inputValidator, logger)
        {
        }
    }

    internal class StoringComponentsDbRepository : StoringEfDataRepository<IComponentSpecification>
    {
        public StoringComponentsDbRepository(IRepositoryInputValidator inputValidator, IBasicLogger<StoringComponentsDbRepository> logger) : base(inputValidator, logger)
        {
        }
        
    }


    internal class ComponentsDataRepository : DatabaseRepositoryAdapter<IComponentSpecification>
    {
        public ComponentsDataRepository(IEfContextFactory efContextFactory, ILoadingEfDataRepository<IComponentSpecification> loadingRepo, IStoringEfDataRepository<IComponentSpecification> storingRepo) : base(efContextFactory, loadingRepo, storingRepo)
        {
        }
    }
}