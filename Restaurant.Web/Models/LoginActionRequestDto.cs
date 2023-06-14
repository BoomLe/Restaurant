using System.ComponentModel.DataAnnotations;

namespace Restaurant.Web.Models
{
    public class LoginActionRequestDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string UserName { set; get; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { set; get; }
    }
}
