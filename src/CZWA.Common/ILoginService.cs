using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Common
{
    public interface ILoginService
    {
        Task<IEntity> GetUser();
        //Task<UserViewModel> Auth(string username, string password);
        Task<IEnumerable<IEntity>> GetRoles();
    }
}
