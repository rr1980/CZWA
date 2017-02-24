using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.DB;
using CZWA.Entitys;
using CZWA.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;

namespace CZWA.Services
{
    public class AccountService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public UserViewModel User
        {
            get
            {
                return _getUser().Result;
            }
        }

        private List<UserViewModel> _allusers;
        public List<UserViewModel> AllUsers
        {
            get
            {
                if (_allusers == null)
                {
                    _allusers = _getAllUsers().Result;
                }
                return _allusers;
            }
            private set
            {
                _allusers = value;
            }
        }


        public AccountService(DataContext context, IHttpContextAccessor httpContextAccessor, ILogger<AccountService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _logger.LogWarning("AccountService init...");
        }

        public bool HasRole(UserRoleType urt)
        {
            return User.Roles.Any(r => r == (int)urt);
        }


        private async Task<List<UserViewModel>> _getAllUsers()
        {
            var users = await _context.GetAllUsers();

            var result = users.Select(user => new UserViewModel()
            {
                UserId = user.UserId,
                Username = user.Username,
                ShowName = user.Username,
                Name = user.Name,
                Vorname = user.Vorname,
                Roles = _getRoles(user)
            }).ToList();

            return result;
        }

        private async Task<UserViewModel> _getUser()
        {
            var id = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            User user = await _context.GetUserById(id);

            return new UserViewModel()
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Vorname = user.Vorname,
                Roles = _getRoles(user)
            };
        }


        private IEnumerable<int> _getRoles(User user)
        {
            var roles = user.RoleToUsers.Select(r => r.Role);
            return roles.Select(r => (int)r.UserRoleType);
        }

        public async Task<bool> Auth(string username, string password)
        {
            var user = await _context.GetUser(username, password);

            if (user != null)
            {
                var claims = new List<Claim> {
                                 new Claim(ClaimTypes.Authentication, "true"),
                                 new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                                 new Claim(ClaimTypes.Surname, user.Name),
                                 new Claim(ClaimTypes.GivenName, user.Vorname),
                                 new Claim(ClaimTypes.Name, user.Username)
                        };

                var uroles = user.RoleToUsers.Select(r => new Claim(ClaimTypes.Role, r.Role.UserRoleType.ToString()));
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

                return true;
            }
            return false;
        }

        public async Task<List<UserViewModel>> DelUser(UserViewModel user)
        {
            var usr = await _context.GetUserById(user.UserId);
            _delRoles(usr);
            _context.Remove(usr);
            _context.SaveChanges();
            return await _getAllUsers();
        }

        public async Task<List<UserViewModel>> SaveUser(UserViewModel user)
        {
            var usr = await _context.GetUserById(user.UserId);

            if (usr == null)
            {
                usr = new User();
                usr = _updateUser(usr, user);
            }
            else
            {
                usr = _delRoles(usr);
                usr = _updateUser(usr, user);
            }

            _context.SaveChanges();

            return await _getAllUsers();
        }

        private User _updateUser(User usr, UserViewModel user)
        {
            usr.Name = user.Name;
            usr.Vorname = user.Vorname;
            usr.Username = user.Username;

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    var rtu = new RoleToUser()
                    {
                        Role = role != -1 ? _context.Roles.First(r => r.UserRoleType == (UserRoleType)role) : _context.Roles.First(r => r.UserRoleType == UserRoleType.Default),
                        User = usr
                    };

                    _context.RoleToUsers.Add(rtu);
                }
            }

            return usr;
        }

        private User _delRoles(User user)
        {
            var rtus = _context.RoleToUsers.Where(rtu => rtu.UserId == user.UserId);
            _context.RemoveRange(rtus);
            return user;
        }
    }
}
