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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CZWA.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILogger _logger;
        private readonly AccountService _accountService;

        public AdminController(ILogger<HomeController> logger, AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            var result = await _accountService.GetAllUsers();
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return View(new AdminViewModel()
            {
                Users = result
            });
        }

        public async Task<AdminViewModel> SaveUser(UserViewModel user)
        {
            List<UserViewModel> result;

            if (!ModelState.IsValid)
            {
                result = await _accountService.GetAllUsers();
                result.Insert(0, new UserViewModel()
                {
                    UserId = -1,
                    ShowName = "Neu...",
                    Roles = new int[] { -1 }
                });

                return new AdminViewModel()
                {
                    Users = result,
                    Errors = GetModelStateErrors(ModelState)
                };
            }

            result = await _accountService.SaveUser(user);
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return new AdminViewModel()
            {
                Users = result,
            };
        }

        public async Task<AdminViewModel> DelUser(UserViewModel user)
        {
            var result = await _accountService.DelUser(user);
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return new AdminViewModel()
            {
                Users = result,
            };
        }

        public IActionResult Error()
        {
            return View();
        }


        public List<string> GetModelStateErrors(ModelStateDictionary ModelState)
        {
            List<string> errorMessages = new List<string>();

            var validationErrors = ModelState.Values.Select(x => x.Errors);
            validationErrors.ToList().ForEach(ve =>
            {
                var errorStrings = ve.Select(x => x.ErrorMessage);
                errorStrings.ToList().ForEach(em =>
                {
                    errorMessages.Add(em);
                });
            });

            return errorMessages;
        }
    }
}



//_logger.LogWarning("loulou");
//_logger.LogError("loulou");
//_logger.LogWarning(LoggingEvents.GET_ITEM, "Getting item {ID}", 1);