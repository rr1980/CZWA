using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.Entitys;

namespace CZWA.DB
{
    public static class SeedData
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var r1 = new Role();
            var r2 = new Role();

            r1.UserRoleType = UserRoleType.Admin;
            r2.UserRoleType = UserRoleType.Default;

            var u1 = new User
            {
                Name = "Riesner",
                Vorname = "Rene",
                Username = "rr1980",
                Password = "12003"
            };
            var rtu1_1 = new RoleToUser()
            {
                Role = r1,
                User = u1
            };

            var rtu1_2 = new RoleToUser()
            {
                Role = r2,
                User = u1
            };

            var u2 = new User
            {
                Name = "Riesner",
                Vorname = "Sven",
                Username = "Oxi",
                Password = "12003"
            };


            var rtu2_1 = new RoleToUser()
            {
                Role = r2,
                User = u2
            };

            context.RoleToUsers.Add(rtu1_1);
            context.RoleToUsers.Add(rtu1_2);
            context.RoleToUsers.Add(rtu2_1);

            context.SaveChanges();

        }
    }
}
