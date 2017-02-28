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
using CZWA.Services;

namespace CZWA.Web.ViewComponents
{
    public class NavbarComponent : ViewComponent
    {
        private readonly AccountService _accountService;
        private readonly ILogger _logger;
        private readonly HttpContext _httpContext;

        public NavbarComponent(AccountService accountService, IHttpContextAccessor httpContextAccessor, ILogger<NavbarComponent> logger)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _logger.LogWarning("NavbarComponent init...");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            return View(new NavbarViewModel()
            {
                UserViewModel = await _accountService.GetUserById(id)
            });
        }
    }
}
