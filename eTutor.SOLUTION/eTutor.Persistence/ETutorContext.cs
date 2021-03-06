﻿using System;
using eTutor.Core.Configurations;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence
{
    public class ETutorContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<ParentStudent> ParentStudents { get; set;}
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TutorSubject> TutorSubjects { get; set; }
        public DbSet<TopicInterest> TopicInterests { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<ParentAuthorization> ParentAuthorizations { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<EmailValidation> EmailValidations { get; set; }
        public DbSet<ChangePassword> ChangePasswordRequests { get; set; }
        public DbSet<RejectedMeeting> RejectedMeetings { get; set; }
        public ETutorContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<UserLogin>().ToTable("UserLogins");
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            
            ApplyConfigurations(builder);
        }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MeetingConfiguration());
            modelBuilder.ApplyConfiguration(new ParentAuthorizationConfiguration());
            modelBuilder.ApplyConfiguration(new ParentStudentConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingConfiguration());
            modelBuilder.ApplyConfiguration(new RejectedMeetingConfiguration());
        }
    }
}
