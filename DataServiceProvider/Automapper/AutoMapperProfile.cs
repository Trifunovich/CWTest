using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DataAccess.Core.Abstractions;
using DataServiceProvider.Core.DtoAbstraction;

namespace DataServiceProvider.Core.Automapper
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap(typeof(IDataModel), typeof(IDto)).AfterMap(AfterMap1);
            CreateMap(typeof(IDto), typeof(IDataModel)).AfterMap(AfterMap2);
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
