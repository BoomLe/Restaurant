using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Restaurant.Service.APIReward.Data;
using Restaurant.Service.APIReward.Messages;
using Restaurant.Service.APIReward.Models;
using Restaurant.Service.APIReward.Services.IServices;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Service.APIEmail.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<ApplicationDBContext> _dbOptions;

        public RewardService(DbContextOptions<ApplicationDBContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }


        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.Now
                };
                await using var _db = new ApplicationDBContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
