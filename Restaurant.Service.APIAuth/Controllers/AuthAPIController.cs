using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restaurant.AzureBusMessage.Messages;
using Restaurant.Service.APIAuth.Models.DTO;
using Restaurant.Service.APIAuth.Services.IServices;
using System.Threading.Tasks;

namespace Restaurant.Service.APIAuth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IServiceAuthAPI _serviceAuthAPI;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDto _responseDto;
        public AuthAPIController(IServiceAuthAPI serviceAuthAPI, IConfiguration configuration, IMessageBus messageBus)
        {
            _configuration = configuration;
            _messageBus = messageBus;
            _serviceAuthAPI = serviceAuthAPI;
            _responseDto = new ();
        }
        [HttpPost("login")]
        public async Task <IActionResult> Login(LoginActionRequestDto model) 
        {
            var loginPasss = await _serviceAuthAPI.Login(model);
            if(loginPasss.User == null) 
            {
                _responseDto.IsSuccess = false;
                _responseDto.Messages = "Tài khoản và mật khẩu không chính xác !";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginPasss;
            return Ok(_responseDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterActionRequestDto model)
        {
            var erroMessages = await _serviceAuthAPI.Register(model);

            if (!string.IsNullOrEmpty(erroMessages)) 
            {
                _responseDto.IsSuccess = false;
                _responseDto.Messages = erroMessages;
             
                return BadRequest(_responseDto);
            }
            _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
            return Ok(_responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterActionRequestDto model)
        {
            var erroAssignRole = await _serviceAuthAPI.AssignRole(model.Email, model.role);

            if (!erroAssignRole)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Messages = "Lỗi hệ thống !";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
    }
}
