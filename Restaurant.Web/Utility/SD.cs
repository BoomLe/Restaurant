namespace Restaurant.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { set; get; }
        public static string ProductAPIBase { set; get; }
        public static string CartAPIBase { set; get; }
        public static string AuthAPIBase { set; get; }
        public static string OrderAPIBase { set; get; }

        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "JwtToken";
        public enum ApiType 
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";

  
    }
}
