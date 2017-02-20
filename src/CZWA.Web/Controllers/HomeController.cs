using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CZWA.Views.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> Index()
        {
            var nachname = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
            var vorname = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;

            var result = new HomeViewModel() { Name = nachname, Vorname = vorname };

            return View(result);
        }

        [Authorize(Policy = "ReadPolicy")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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