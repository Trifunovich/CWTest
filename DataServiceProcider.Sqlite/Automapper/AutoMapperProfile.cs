using System;
using AutoMapper;
using DataAccess.Core.Abstractions;
using DataAccess.CWTest.Abstraction;
using DataServiceProvider.Core.DtoAbstraction;
using DataServiceProvider.TestBench.Dtos;

namespace DataServiceProvider.TestBench.Automapper
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            RegisterMapping();
        }

        private void RegisterMapping()
        {
            CreateMap<IComponentSpecification, ComponentsDto>().AfterMap(AfterMap1);
            CreateMap<ComponentsDto, IComponentSpecification>().AfterMap(AfterMap2);
        }

        private void AfterMap2<TDto, T>(TDto dto, T original)
        {
            if (dto is IDto dtoC && original is IDataModel dataModel)
            {
                dataModel.SetId(dtoC.IdAbstraction);
            }
        }

        private void AfterMap1<T, TDto>(T original, TDto dto)
        {
            if (dto is IDto dtoC && original is IDataModel dataModel)
            {
                dtoC.SetId(dataModel.IdAbstraction);
            }
        }

    }
}
