﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Entitys;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CZWA.DB
{
    public class DataContext : DbContext
    {
        private readonly ILogger _logger;

        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger=null)
            : base(options)
        {
            if (logger != null)
            {
                _logger = logger;
                _logger.LogWarning("DataContext init...");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleToUser> RoleToUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<RoleToUser>().HasKey(t => new { t.UserId, t.RoleId });
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.Role).WithMany(r => r.RoleToUsers).HasForeignKey(rtu => rtu.RoleId);
            modelBuilder.Entity<RoleToUser>().HasOne(rtu => rtu.User).WithMany(r => r.RoleToUsers).HasForeignKey(rtu => rtu.UserId);
        }

        public async Task<User> GetUserById(int id)
        {
            var usr = (User)await Users.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.UserId == id);
            return usr;
        }

        public async Task<User> GetUser(string username, string password)
        {
            var usr = (User)await Users.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
            return usr;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var usrs = await Users.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).ToListAsync();
            return usrs;
        }
    }
}
