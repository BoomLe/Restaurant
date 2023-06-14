namespace Restaurant.Service.APIReward.Messages
{
    public class RewardsMessage
    {
        public string UserId { set; get; }
        public int RewardsActivity { set; get; }
        public int OrderId { set; get; }
    }
}
