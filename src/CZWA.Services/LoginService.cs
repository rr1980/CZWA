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
using Microsoft.Extensions.Logging;

namespace CZWA.Services
{
    public class LoginService : ILoginService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public LoginService(DataContext context, IHttpContextAccessor httpContextAccessor, ILogger<LoginService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _logger.LogWarning("LoginService init...");
        }

        public async Task<IEntity> GetUser()
        {
            var id = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            User user = await _context.GetUserById(id);

            //var nachname = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
            //var vorname = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;

            //var result = new UserViewModel() { Name = nachname, Vorname = vorname };

            return user;
        }

        public async Task<IEnumerable<IEntity>> GetRoles()
        {
            return ((User)await GetUser()).RoleToUser.Select(rtu => rtu.Role);
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
