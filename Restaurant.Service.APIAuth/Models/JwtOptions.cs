namespace Restaurant.Service.APIAuth.Models
{
    public class JwtOptions
    {
        public string Secret { set; get; } = string.Empty;
        public string Issuer { set; get; } = string.Empty;
        public string Audience { set; get; } = string.Empty;
    }
}
