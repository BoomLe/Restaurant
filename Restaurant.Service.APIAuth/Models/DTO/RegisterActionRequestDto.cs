namespace Restaurant.Service.APIAuth.Models.DTO
{
    public class RegisterActionRequestDto
    {
        public string Email { set; get; }
        public string Name { set; get; }
        public string PhoneNumber { set; get; }

        public string Password { set; get; }

        public string? role { set; get; }
    }
}
