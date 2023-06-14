using Restaurant.Service.APICart.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Service.APICart.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
