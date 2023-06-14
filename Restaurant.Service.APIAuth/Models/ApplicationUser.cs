using Microsoft.AspNetCore.Identity;

namespace Restaurant.Service.APIAuth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { set; get; }
    }
}
