using System;
using eTutor.Core.Configurations;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence
{
    public class ETutorContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<ParentStudent> ParentStudents { get; set;}
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TutorTopic> TutorTopics { get; set; }
        public DbSet<TopicInterest> TopicInterests { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<ParentAutorization> ParentAutorizations { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

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
            modelBuilder.ApplyConfiguration(new ParentConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new TutorConfiguration());
            modelBuilder.ApplyConfiguration(new UserConiguration());
        }
    }
}
