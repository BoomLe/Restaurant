namespace Restaurant.Service.APIOrder.Models.Dto
{
    public class StripeRequestDto
    {
        public string? StripeSeesionUrl { set; get; }

        public string? StripeSessionId { set; get; }

        public string ApproveUrl { set; get; }

        public string CancelUrl { set; get; }

        public OrderHeaderDto OrderHeader { set; get; }
    }
}
