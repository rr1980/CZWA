using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;
using Microsoft.AspNetCore.Authorization;

namespace CZWA.Authorization
{
    public class AuthPolicyRequirement : IAuthorizationRequirement
    {
        public UserRoleType UserRoleType;

        public AuthPolicyRequirement(UserRoleType type)
        {
            this.UserRoleType = type;
        }
    }
}
