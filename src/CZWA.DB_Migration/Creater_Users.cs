using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.DB;
using CZWA.Entitys;

namespace CZWA.DB_Migration
{
    public static class Creater_Users
    {
        internal static void Create(DataContext context, UserRoleType[] roles, User user)
        {
            foreach (var role in roles)
            {
                var rtu = new RoleToUser();

                var ro = context.Roles.First(r => r.UserRoleType == role);

                rtu.Role = ro;
                rtu.User = user;

                context.RoleToUsers.Add(rtu);
            }

            context.SaveChanges();
        }
    }
}
