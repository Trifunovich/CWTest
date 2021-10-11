using System;
using AutoMapper;
using DataAccess.Core.Abstractions;
using DataServiceProvider.Core.DtoAbstraction;

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
            CreateMap<IDataModel, IDto>().AfterMap(AfterMap1);
            CreateMap<IDto, IDataModel>().AfterMap(AfterMap2);
        }

        private void AfterMap1(IDataModel dataModel, IDto dto)
        {
            dto.Id = dataModel.IdAbstraction;
        }

        private void AfterMap2(IDto dto, IDataModel original)
        {
            original.SetId(dto.Id);
        }

    }
}
