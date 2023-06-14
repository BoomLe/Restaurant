using Restaurant.Web.Models;
using System.Threading.Tasks;

namespace Restaurant.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginActionRequestDto loginActionRequestDto);
        Task<ResponseDto?> RegisterAsync(RegisterActionRequestDto registerActionRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegisterActionRequestDto registerActionRequestDto);
    }
}
