using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;
using DataAccess.CWTest.Abstraction;
using DataAccess.Sql.Models;
using DataServiceProvider.Core.UnitOfWork;
using DataServiceProvider.TestBench.Dtos;
using LoggingLibrary;

namespace DataServiceProvider.TestBench.Services
{
    public interface IComponentsService : IUnitOfWork<ComponentsDto>
    {

    }

    internal class ComponentsService : UnitOfWorkBase<ComponentsDto, IComponentSpecification>, IComponentsService
    {
        public ComponentsService(IBasicLogger<ComponentsService> logger, ILoadingDataRepository<IComponentSpecification> loadRepo, IStoringDataRepository<IComponentSpecification> storeRepo, IMapper mapper) : base(logger, loadRepo, storeRepo, mapper)
        {
        }
    }
}
