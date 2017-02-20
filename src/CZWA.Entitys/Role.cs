using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;

namespace CZWA.Entitys
{
    public class Role : IEntity
    {
        public int RoleId { get; set; }
        public UserRoleType UserRoleType { get; set; }

        public virtual ICollection<RoleToUser> RoleToUser { get; set; }
    }
}
