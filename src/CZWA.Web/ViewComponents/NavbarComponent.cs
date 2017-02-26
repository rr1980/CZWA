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

        public NavbarComponent(AccountService accountService, ILogger<NavbarComponent> logger)
        {
            _accountService = accountService;
            _logger = logger;
            _logger.LogWarning("NavbarComponent init...");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new NavbarViewModel()
            {
                UserViewModel = await _accountService.GetUser()
            });
        }
    }
}
