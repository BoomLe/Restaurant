using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Service.APIProduct.Data;
using Restaurant.Service.APIProduct.Models;
using Restaurant.Service.APIProduct.Models.Dto;
using Restaurant.Service.APIProduct.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant.Service.APIProduct.Controllers
{
    [Route("api/product")]
    [ApiController]

    public class ProductAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private  ResponseDto _response;
        private  IMapper _mapper;
        public ProductAPIController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            
        }

        [HttpGet]
        public ResponseDto Get() 
        {
            try 
            {
                IEnumerable<Product> Products = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(Products);
              
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product Products = _db.Products.First(y=> y.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(Products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }


		[HttpPost]
		[Authorize(Roles = "Admin")]
        public ResponseDto Post([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _db.Products.Add(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDto Put([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product Products = _mapper.Map<Product>(ProductDto);
                _db.Products.Update(Products);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(Products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto Delete(int id)
        {
            try
            {
              Product Products = _db.Products.First(p => p.ProductId == id);
                _db.Products.Remove(Products);
                _db.SaveChanges();
     
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }
    }
}
