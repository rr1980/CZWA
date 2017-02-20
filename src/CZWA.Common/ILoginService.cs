using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.Common
{
    public interface ILoginService
    {
        Task<IEntity> Auth(string username, string password);
    }
}
