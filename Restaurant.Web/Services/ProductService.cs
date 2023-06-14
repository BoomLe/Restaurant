

using Restaurant.Web.Models;

using Restaurant.Web.Services.IServices;
using Restaurant.Web.Utility;
using System.Threading.Tasks;
using static Restaurant.Web.Utility.SD;

namespace Restaurant.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = productDto,
                Url = ProductAPIBase + "/api/product" 
             
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product"
            });
        }

        //public async Task<ResponseDto?> GetProductAsync(string productCode)
        //{
        //    return await _baseService.SendAsync(new RequestDto() 
        //    {
        //        ApiType = ApiType.GET,
        //        Url = ProductAPIBase + "/api/product/GetByCode/" + productCode,
        //    });
        //}

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = couponDto,
                Url = ProductAPIBase + "/api/product" 
               
            });
        }
    }
}
