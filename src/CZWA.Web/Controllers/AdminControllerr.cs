using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CZWA.ViewModels;

namespace CZWA.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILogger _logger;

        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            return View(new AdminViewModel());
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