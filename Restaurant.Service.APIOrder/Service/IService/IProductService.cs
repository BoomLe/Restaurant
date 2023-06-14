
using Restaurant.Service.APIOrder.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Service.APIOrder.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
