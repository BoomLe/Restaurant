using System;

namespace Restaurant.Service.APIReward.Models
{
    public class Rewards
    {
        public int Id { set; get; }
        public string UserId { set; get; }
        public DateTime RewardsDate { set; get; }
        public int RewardsActivity { set; get; }
        public int OrderId { set; get;}
    }
}
