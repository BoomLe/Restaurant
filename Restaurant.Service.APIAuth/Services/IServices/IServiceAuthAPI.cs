using Restaurant.Service.APIAuth.Models.DTO;
using System.Threading.Tasks;

namespace Restaurant.Service.APIAuth.Services.IServices
{
    public interface IServiceAuthAPI
    {
        Task<string> Register(RegisterActionRequestDto requestDto);
        Task<LoginResponseDto> Login(LoginActionRequestDto requestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
