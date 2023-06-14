namespace Restaurant.Service.APIEmail.Messages
{
    public class RewardsMessage
    {
        public string UserId { set; get; }
        public int RewardsActivity { set; get; }
        public int OrderId { set; get; }
    }
}
