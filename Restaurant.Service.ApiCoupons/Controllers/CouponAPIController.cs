using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Service.ApiCoupons.Data;
using Restaurant.Service.ApiCoupons.Models;
using Restaurant.Service.ApiCoupons.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Service.ApiCoupons.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private  ResponseDto _response;
        private  IMapper _mapper;
        public CouponAPIController(ApplicationDBContext db, IMapper mapper)
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
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
              
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
                Coupon coupons = _db.Coupons.First(y=> y.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(coupons);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupons = _db.Coupons.First(y => y.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupons);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupons = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupons);
                _db.SaveChanges();


                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long)(couponDto.DiscountAmount*100),
                    Name = couponDto.CouponCode,
                    Currency ="usd",
                    Id =couponDto.CouponCode

                };
                var service = new Stripe.CouponService();
                service.Create(options);

                _response.Result = _mapper.Map<CouponDto>(coupons);
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
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupons = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(coupons);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupons);
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
              Coupon coupons = _db.Coupons.First(p => p.CouponId == id);
                _db.Coupons.Remove(coupons);
                _db.SaveChanges();

                var service = new Stripe.CouponService();
                service.Delete(coupons.CouponCode);
     
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
