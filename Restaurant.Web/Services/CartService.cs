

using Restaurant.Web.Models;

using Restaurant.Web.Services.IServices;
using Restaurant.Web.Utility;
using System.Threading.Tasks;
using static Restaurant.Web.Utility.SD;

namespace Restaurant.Web.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = CartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = CartAPIBase + "/api/cart/emailcartrequest"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsnyc(string userId)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.GET,
                Url = CartAPIBase + "/api/cart/GetCart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.POST,
                Data = cartDetailsId,
                Url = CartAPIBase + "/api/cart/removecart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = CartAPIBase + "/api/cart/CartUpsert"
            });
        }
    }
}
