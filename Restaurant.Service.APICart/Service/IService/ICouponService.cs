using Restaurant.Service.APICart.Models.Dto;
using System.Threading.Tasks;

namespace Restaurant.Service.APICart.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
