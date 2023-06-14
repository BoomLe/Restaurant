using Restaurant.Service.APIEmail.Messages;
using Restaurant.Service.APIEmail.Models.Dto;
using System.Threading.Tasks;

namespace Restaurant.Service.APIEmail.Services.IServices
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
        Task LogOrderPlaced(RewardsMessage rewardsDto);
    }
}
