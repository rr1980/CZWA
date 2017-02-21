using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CZWA.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Task<UserViewModel> _getUser()
        {
            var nachname = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value;
            var vorname = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;

            var result = new UserViewModel() { Name = nachname, Vorname = vorname };

            return Task.FromResult(result);
        }
    }
}