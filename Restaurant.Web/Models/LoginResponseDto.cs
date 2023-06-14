namespace Restaurant.Web.Models
{
    public class LoginResponseDto
    {
        public UserDto User { set; get; }
        public string Token { set; get; }
    }
}
