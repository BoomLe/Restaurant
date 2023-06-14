
using Newtonsoft.Json;
using Restaurant.Service.APIOrder.Models.Dto;
using Restaurant.Service.APIOrder.Service.IService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Restaurant.Service.APIOrder.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var respo = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
			if (respo.IsSuccess)
			{
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(respo.Result));

            }

            return new List<ProductDto>();
        }
    }
}
