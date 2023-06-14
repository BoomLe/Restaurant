using Microsoft.AspNetCore.Identity;
using Restaurant.Service.APIAuth.Data;
using Restaurant.Service.APIAuth.Models;
using Restaurant.Service.APIAuth.Models.DTO;
using Restaurant.Service.APIAuth.Services.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Service.APIAuth.Services
{
    public class ServiceAuthAPI : IServiceAuthAPI
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public ServiceAuthAPI(ApplicationDBContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManger,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _db = db;
            _userManager = userManager;
            _roleManger = roleManger;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(p => p.Email.ToLower() == email.ToLower());
            if(user != null) 
            {
                if (!_roleManger.RoleExistsAsync(roleName).GetAwaiter().GetResult()) 
                {
                    _roleManger.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginActionRequestDto requestDto)
        {
            var loginPass = _db.ApplicationUsers.FirstOrDefault(p => p.UserName.ToLower() == requestDto.UserName.ToLower());
            var checkPass = await _userManager.CheckPasswordAsync(loginPass, requestDto.Password);
            if (loginPass == null || checkPass == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }
            //JWt Token
            var roles = await _userManager.GetRolesAsync(loginPass);
            var token = _jwtTokenGenerator.GenerateToken(loginPass, roles);
            UserDto userDto = new()
            {
                Email = loginPass.Email,
                Id = loginPass.Id,
                PhoneNumber = loginPass.PhoneNumber,
                Name = loginPass.Name
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegisterActionRequestDto requestDto)
        {
            ApplicationUser user = new()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email,
                NormalizedEmail = requestDto.Email.ToUpper(),
                PhoneNumber = requestDto.PhoneNumber,
                Name = requestDto.Name
            };

            try
            {
                var result = await _userManager.CreateAsync(user, requestDto.Password);
                if (result.Succeeded)
                {
                    var userReturnDto = _db.ApplicationUsers.First(p => p.UserName == requestDto.Email);
                    UserDto userDto = new()
                    {
                        Id = userReturnDto.Id,
                        Name = userReturnDto.Name,
                        Email = userReturnDto.Email,
                        PhoneNumber = userReturnDto.PhoneNumber

                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Lỗi hệ thống !";
        }
    }
}
