using CZWA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CZWA.ViewModels
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel(UserRoleType userRoleType)
        {
            UserRoleType = userRoleType;
        }

        public UserRoleType UserRoleType { get; set; }
    }
}
