using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models
{
    public class RegisterActionRequestDto
    {
        [Required(ErrorMessage = "Bạn hãy nhập Email ")]
        public string Email { set; get; }
        [Required(ErrorMessage = "Bạn chưa nhập tên")]
        public string Name { set; get; }
        [Required(ErrorMessage = " Bạn vui lòng thêm số điện thoại")]
        public string PhoneNumber { set; get; }
        [Required(ErrorMessage = " vui lòng nhập mật khẩu ")]
        public string Password { set; get; }

        public string? role { set; get; }
    }
}
