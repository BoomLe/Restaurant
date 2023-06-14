using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;
using Restaurant.Web.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurant.Web.Controllers
{

    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginActionRequestDto loginActionRequestDto = new();
            return View(loginActionRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginActionRequestDto loginActionRequestDto) 
        {
            ResponseDto responseDto = await _authService.LoginAsync(loginActionRequestDto);
            if(responseDto != null && responseDto.IsSuccess) 
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Messages;
                return View(loginActionRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Register(RegisterActionRequestDto registerActionRequestDto)
        {
            ResponseDto result = await _authService.RegisterAsync(registerActionRequestDto);
            ResponseDto assignRole;

            if(result != null && result.IsSuccess) 
            {
                if (string.IsNullOrEmpty(registerActionRequestDto.role)) 
                {
                    registerActionRequestDto.role = SD.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(registerActionRequestDto);
                if(assignRole != null && assignRole.IsSuccess) 
                {
                    TempData["success"] = "Đăng ký thành công !";
                    return RedirectToAction(nameof(Login));
                }
            }
            else 
            {
                TempData["error"] = result.Messages;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer, Value =SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View(registerActionRequestDto);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model) 
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            //JWT Register
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, 
                jwt.Claims.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.Name).Value));

            //Claim Role
            identity.AddClaim(new Claim(ClaimTypes.Name,
               jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
          jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
