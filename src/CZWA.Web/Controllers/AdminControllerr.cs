using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CZWA.ViewModels;
using CZWA.Services;

namespace CZWA.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILogger _logger;
        private readonly LoginService _loginService;

        public AdminController(ILogger<HomeController> logger, LoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Index()
        {
            return View(new AdminViewModel()
            {
                Users = _loginService.AllUsers
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}



//_logger.LogWarning("loulou");
//_logger.LogError("loulou");
//_logger.LogWarning(LoggingEvents.GET_ITEM, "Getting item {ID}", 1);