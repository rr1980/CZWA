﻿using System;
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
    public class LoginService 
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        //private UserViewModel _user;
        public UserViewModel User
        {
            get
            {
                //if (_user == null)
                //{
                //    _user = _getUser().Result;
                //}
                return _getUser().Result;
            }
            //private set
            //{
            //    _user = value;
            //}
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


        public LoginService(DataContext context, IHttpContextAccessor httpContextAccessor, ILogger<LoginService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _logger.LogWarning("LoginService init...");
        }

        public bool HasRole(UserRoleType urt)
        {
            return User.Roles.Any(r => r == (int)urt);
        }


        private async Task<List<UserViewModel>> _getAllUsers()
        {
            var result = await _context.GetAllUsers();

            return result.Select(user => new UserViewModel()
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Vorname = user.Vorname,
                Roles = _getRoles(user)
            }).ToList();
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
    }
}
