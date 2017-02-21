using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CZWA.Common;
using CZWA.Entitys;
using Microsoft.Extensions.Logging;

namespace CZWA.Web.ViewComponents
{
    public class NavbarComponent : ViewComponent
    {
        private readonly ILoginService _loginService;
        private readonly ILogger _logger;

        public NavbarComponent(ILoginService loginService, ILogger<NavbarComponent> logger)
        {
            _loginService = loginService;
            _logger = logger;
            _logger.LogWarning("NavbarComponent init...");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            User usr = (User)await _loginService.GetUser();
            var uroles = (IEnumerable<Role>) await _loginService.GetRoles();

            return View(new NavbarViewModel()
            {
                UserViewModel = new UserViewModel() { Name = usr.Name, Vorname = usr.Vorname, Username = usr.Username , Roles= uroles.Select(r=>new UserRoleViewModel(r.UserRoleType))}
            });
        }
    }
}
