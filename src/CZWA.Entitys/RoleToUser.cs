using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;

namespace CZWA.Entitys
{
    public class RoleToUser : IEntity
    {
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
