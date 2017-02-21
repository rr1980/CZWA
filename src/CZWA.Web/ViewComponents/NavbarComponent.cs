using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CZWA.Web.ViewComponents
{
    public class NavbarComponent : ViewComponent
    {
#pragma warning disable CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        public async Task<IViewComponentResult> InvokeAsync(UserViewModel User)
#pragma warning restore CS1998 // Bei der asynchronen Methode fehlen "await"-Operatoren. Die Methode wird synchron ausgeführt.
        {
            var result = new NavbarViewModel()
            {
                User = User
            };

            return View(result);
        }
    }
}
