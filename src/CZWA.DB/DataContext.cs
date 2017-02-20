using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Entitys;
using Microsoft.EntityFrameworkCore;

namespace CZWA.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleToUser> RoleToUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleToUser>().HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.Role).WithMany(r => r.RoleToUser).HasForeignKey(rtu => rtu.RoleId);
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.User).WithMany(r => r.RoleToUser).HasForeignKey(rtu => rtu.UserId);
        }

        public async Task<User> GetUser(string username, string password)
        {
            var usr = (User) await Users.Include(u => u.RoleToUser).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
            return usr;
        }
    }
}
