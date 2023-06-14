using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Restaurant.AzureBusMessage.Messages;
using Restaurant.Service.APICart.Data;
using Restaurant.Service.APICart.Models;
using Restaurant.Service.APICart.Models.Dto;
using Restaurant.Service.APICart.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Service.APICart.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDBContext _db;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private IConfiguration _configuration;
        
        public CartAPIController(ApplicationDBContext db, IMapper mapper,
            ICouponService couponService
            ,IProductService productService,
            IMessageBus messageBus,
             IConfiguration configuration)
        {
            //CTOR
            _db = db;
            _mapper = mapper;
            this._response = new ResponseDto();
            _productService = productService;
            _couponService = couponService;
            _messageBus = messageBus;
            _configuration = configuration;
            
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId) 
        {
            try 
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(p=>p.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                    .Where(p=>p.CartHeaderId == cart.CartHeader.CartHeaderId));

                //IproductService
                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach(var item in cart.CartDetails) 
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                // Coupon apply
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode)) 
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount) 
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody]CartDto cartDto) 
        {
            try 
            {
                var carFromDb = await _db.CartHeaders.FirstAsync(p => p.UserId == cartDto.CartHeader.UserId);
                carFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(carFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.Messages = ex.ToString();
            }
            return _response;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {
                await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.ToString();
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto cartDto) 
        {
            try 
            {
                var cardFromDb = await _db.CartHeaders.FirstAsync(p => p.UserId == cartDto.CartHeader.UserId);
                cardFromDb.CouponCode = "";
                _db.CartHeaders.Update(cardFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;

            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.Messages = ex.ToString();
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto) 
        {
            try 
            {
                var cartHeaderDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == cartDto.CartHeader.UserId);
                if(cartHeaderDb == null) 
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var cartDetailsDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        p=> p.ProductId == cartDto.CartDetails.First().ProductId &&
                        p.CartHeaderId == cartHeaderDb.CartHeaderId);
                    if(cartDetailsDb == null) 
                    {
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else 
                    {
                        cartDto.CartDetails.First().Count += cartDetailsDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Messages = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try 
            {
                CartDetails cartDetails = _db.CartDetails
                    .First(p => p.CartDetailsId == cartDetailsId);

                int totalCountoCartItem = _db.CartDetails.Where(p => p.CartDetailsId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if(totalCountoCartItem == 1) 
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                        .FirstOrDefaultAsync(p => p.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Messages = ex.Message.ToString();
                _response.IsSuccess = false;

            }
            return _response;
        }
       
    }
}
