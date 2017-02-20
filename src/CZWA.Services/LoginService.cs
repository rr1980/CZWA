using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.DB;
using CZWA.Entitys;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace CZWA.Services
{
    public class LoginService : ILoginService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IEntity> Auth(string username, string password)
        {
            User user = await _context.GetUser(username,password);
            if (user != null)
            {
                var claims = new List<Claim> {
                                 new Claim(ClaimTypes.Authentication, "true"),
                                 new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                                 new Claim(ClaimTypes.Surname, user.Name),
                                 new Claim(ClaimTypes.GivenName, user.Vorname),
                                 new Claim(ClaimTypes.Name, user.Username)
                        };

                var uroles = user.RoleToUser.Select(rtu => rtu.Role).Select(r => new Claim(ClaimTypes.Role, r.UserRoleType.ToString()));
                foreach (var role in uroles)
                {
                    claims.Add(role);
                }


                var claimsIdentity = new ClaimsIdentity(claims, "password");
                var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

                
                await _httpContextAccessor.HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(12),
                    IsPersistent = false,
                    AllowRefresh = true
                });

            }

            return user;
        }
    }
}
