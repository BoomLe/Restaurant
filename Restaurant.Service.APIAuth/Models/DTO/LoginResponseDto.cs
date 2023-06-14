namespace Restaurant.Service.APIAuth.Models.DTO
{
    public class LoginResponseDto
    {
        public UserDto User { set; get; }
        public string Token { set; get; }
    }
}
