
using Restaurant.Service.APIReward.Messages;
using System.Threading.Tasks;

namespace Restaurant.Service.APIReward.Services.IServices
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
        
    }
}
