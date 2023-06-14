using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;

using Restaurant.Service.APIProduct.Models;
using Restaurant.Service.APIProduct.Models.Dto;

namespace Restaurant.Service.APIProduct
{
    public class MapperConfig : Profile
    {
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<ProductDto, Product>().ReverseMap();
			});
			return mappingConfig;
		}
	}
}
