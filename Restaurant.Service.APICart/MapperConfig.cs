using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Restaurant.Service.APICart.Models;
using Restaurant.Service.APICart.Models.Dto;

namespace Restaurant.Service.APICart
{
    public class MapperConfig : Profile
    {
        public static MapperConfiguration RegisterMapper()
        {
            var mappingCofig = new MapperConfiguration(options =>
            {
                options.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                options.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();

            });
            return mappingCofig;
        }
    }
}
