using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Service.APIOrder.Data;
using Restaurant.Service.APIOrder.Models;
using Restaurant.Service.APIOrder.Models.Dto;
using Restaurant.Service.APIOrder.Service.IService;
using Restaurant.Service.APIOrder.Utility;
using Stripe.Checkout;
using Stripe;
using Restaurant.AzureBusMessage.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Restaurant.Service.APIOrder.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDBContext _db;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public OrderAPIController(ApplicationDBContext db, IProductService productService,
            IMapper mapper, IConfiguration configuration, IMessageBus messageBus)
        {
            _db = db;
            this._response = new ResponseDto();
            _productService = productService;
            _mapper = mapper;
            _messageBus = messageBus;
            _configuration = configuration;

        }
        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderHeader = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderHeader.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = ex.Message;
            }
            return _response;
        }
        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {


                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApproveUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),

                    Mode = "payment",
                };

                var DiscountsObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon = stripeRequestDto.OrderHeader.CouponCode
                    }
                };

                foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }

                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountsObj;
                }

                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSeesionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(p => p.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _db.SaveChanges();
                _response.Result = stripeRequestDto;

            }
            catch (Exception ex)
            {
                _response.Messages = ex.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(p => p.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntenService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntenService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _db.SaveChanges();

                    //Rewards
                    RewardsDto rewardsDto = new()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardsActivity = Convert.ToInt32(orderHeader.OrderDetails),
                        UserId = orderHeader.UserId
                    };

                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                  await  _messageBus.PublishMessage(rewardsDto, topicName);

                    _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _response.Messages = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;


        }

        [Authorize]
        [HttpPost("GetOrders")]

        public async  Task< ResponseDto?> Get(string? userId = "")
        {
            try 
            {
                IEnumerable<OrderHeader> objList;
                if (User.IsInRole(SD.RoleAdmin)) 
                {
                    objList = _db.OrderHeaders.Include(p => p.OrderDetails).OrderByDescending(p => p.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _db.OrderHeaders.Include(p => p.OrderDetails).Where(p=> p.UserId == userId).OrderByDescending(p => p.OrderHeaderId).ToList();

                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);
            }
            catch(Exception ex) 
            {
                _response.Messages = ex.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        
        [HttpGet("GetOrder/{id:int}")]

        public async Task<ResponseDto?> Get(int id)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.Include(p => p.OrderDetails).First(p => p.OrderHeaderId == id);
                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }
            catch (Exception ex)
            {
                _response.Messages = ex.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody] string newStatus) 
        {
            try 
            {
                OrderHeader orderHeader = _db.OrderHeaders.First(p => p.OrderHeaderId == orderId);
                if(orderHeader != null) 
                {
                    if(newStatus == SD.Status_Cancelled) 
                    {
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };

                        var service = new RefundService();
                        Refund refund = service.Create(options);
                        orderHeader.Status = newStatus;
                    }

                            orderHeader.Status = newStatus;
                    _db.SaveChanges();
                }
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
