using Restaurant.Web.Models;
using System.Threading.Tasks;

namespace Restaurant.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<ResponseDto>? SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
