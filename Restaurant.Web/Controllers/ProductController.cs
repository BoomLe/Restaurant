using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
			_productService = productService;
        }
        [Route("ProductIndex")]
        public async Task <IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();
            ResponseDto? response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else 
            {
                TempData["error"] = response?.Messages;
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Messages;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
		{
			ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if(response != null && response.IsSuccess) 
            {
                ProductDto? coupon = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(coupon);
			}
            return NotFound();

		}
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model) 
        {
                ResponseDto? response = await _productService.DeleteProductAsync(model.ProductId);
                if(response != null && response.IsSuccess) 
                {
                TempData["success"] = "Xóa sản phẩm thành công !";
                    return RedirectToAction(nameof(ProductIndex));
                }
			else
			{
				TempData["error"] = response?.Messages;
			}


			return View(model);
		}

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? coupon = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(coupon);
            }
            return NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            ResponseDto? response = await _productService.UpdateProductAsync(model);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật sản phẩm thành công !";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Messages;
            }


            return View(model);
        }
    }
}
