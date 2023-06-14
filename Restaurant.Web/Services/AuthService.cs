using Restaurant.Web.Models;

using Restaurant.Web.Services.IServices;
using System.Threading.Tasks;
using static Restaurant.Web.Utility.SD;

namespace Restaurant.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegisterActionRequestDto registerActionRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto ()
            {
                ApiType = ApiType.POST,
                Data = registerActionRequestDto,
                Url = AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginActionRequestDto loginActionRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.POST,
                Data = loginActionRequestDto,
                Url = AuthAPIBase + "/api/auth/login"
            },withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterActionRequestDto registerActionRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = ApiType.POST,
                Data = registerActionRequestDto,
                Url = AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
