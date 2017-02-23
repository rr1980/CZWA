using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Common
{
    public interface ILoginService
    {
        IEntity User { get; set; }
        //Task<UserViewModel> Auth(string username, string password);
        //Task<IEnumerable<IEntity>> GetRoles();
    }
}
