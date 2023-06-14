using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using Restaurant.Service.ApiCoupons.Models;
using Restaurant.Service.ApiCoupons.Models.DTO;

namespace Restaurant.Service.ApiCoupons
{
    public class MapperConfig : Profile
    {
        public static MapperConfiguration RegisterMapper() 
        {
            var mappingCofig = new MapperConfiguration(options => 
            {
                options.CreateMap<CouponDto, Coupon>();
                options.CreateMap<Coupon, CouponDto>();
            });
            return mappingCofig;
        }
    }
}
