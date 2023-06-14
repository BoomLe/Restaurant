using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Restaurant.Service.APIOrder.Models;
using Restaurant.Service.APIOrder.Models.Dto;

namespace Restaurant.Service.APIOrder
{
    public class MapperConfig : Profile
    {
        public static MapperConfiguration RegisterMapper()
        {
            var mappingCofig = new MapperConfiguration(options =>
            {
                options.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

                options.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

                options.CreateMap<OrderDetailsDto, CartDetailsDto>();

                options.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();

                options.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();


            });
            return mappingCofig;
        }
    }
}
