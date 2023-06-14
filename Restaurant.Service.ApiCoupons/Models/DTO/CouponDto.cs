namespace Restaurant.Service.ApiCoupons.Models.DTO
{
    public class CouponDto
    {
        public int CouponId { set; get; }
        public string CouponCode { set; get; }
        public double DiscountAmount { set; get; }
        public int MinAmount { set; get; }
    }
}
