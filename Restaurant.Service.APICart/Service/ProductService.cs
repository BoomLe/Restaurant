﻿using Newtonsoft.Json;
using Restaurant.Service.APICart.Models.Dto;
using Restaurant.Service.APICart.Service.IService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Restaurant.Service.APICart.Service
{
    public class ProductService : IProductService
    {
        
            private readonly IHttpClientFactory _httpClientFactory;

            public ProductService(IHttpClientFactory clientFactory)
            {
                _httpClientFactory = clientFactory;
            }
            public async Task<IEnumerable<ProductDto>> GetProducts()
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync($"/api/product");
                var apiContet = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
                if (resp.IsSuccess)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
                }
                return new List<ProductDto>();
            }
        }
    }

