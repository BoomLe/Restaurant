using Restaurant.Service.APIAuth.Models;
using System.Collections.Generic;

namespace Restaurant.Service.APIAuth.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
