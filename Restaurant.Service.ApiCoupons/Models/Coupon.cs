using System.ComponentModel.DataAnnotations;

namespace Restaurant.Service.ApiCoupons.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { set; get; }
        [Required]
        public string CouponCode { set; get; }
        [Required]
        public double DiscountAmount { set; get; }
        public int MinAmount { set; get; }

    
    }
}
