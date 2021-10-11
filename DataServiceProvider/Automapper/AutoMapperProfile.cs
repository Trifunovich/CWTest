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
      
        private void RegisterMapping<T, TDto>(Type t)
            where T : IDataModel
            where TDto : IDto
        {
            CreateMap(typeof(T), typeof(TDto)).AfterMap(AfterMap1);
            CreateMap(typeof(TDto), typeof(T)).AfterMap(AfterMap2);
        }

        private void AfterMap2<TDto, T>(TDto dto, T original)
        {
            if (dto is IDto dtoC && original is IDataModel dataModel)
            {
                dataModel.SetId(dtoC.Id);
            }
        }

        private void AfterMap1<T, TDto>(T original, TDto dto)
        {
            if (dto is IDto dtoC && original is IDataModel dataModel)
            {
                dtoC.Id = dataModel.IdAbstraction;
            }
        }
    }
}
