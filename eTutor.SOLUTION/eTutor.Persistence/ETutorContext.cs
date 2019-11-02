using System;
using eTutor.Core.Configurations;
using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence
{
    public class ETutorContext : DbContext
    {

        internal DbSet<User> Users { get; set; }
        internal DbSet<UserRole> UserRoles { get; set; }
        internal DbSet<Role> Roles { get; set; }

        public ETutorContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyConfigurations(modelBuilder);
        }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
