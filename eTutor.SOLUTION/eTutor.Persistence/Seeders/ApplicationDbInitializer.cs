using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence.Seeders
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(IPasswordHasher<User> hasher, ETutorContext context)
        {
            string password = hasher.HashPassword(new User(), "123456");

            string adminEmail = "admin@etutor.com";
            string tutorEmail = "tutor@etutor.com";
            string studentEmail = "student@etutor.com";
            string parentEmail = "parent@etutor.com";

            User admin = context.Users
                .FirstOrDefault(u => u.Email == adminEmail);
            User tutor = context.Users
                .FirstOrDefault(u => u.Email == tutorEmail);
            User student = context.Users
                .FirstOrDefault(u => u.Email == studentEmail);
            User parent = context.Users
                .FirstOrDefault(u => u.Email == parentEmail);
            if (admin == null)
            {
                User user = new User
                {
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Gender = Gender.Male,
                    IsEmailValidated = true,
                    IsTemporaryPassword = false,
                    Name = "Admin",
                    SecurityStamp = "n57iIauoLM1JW9PaV6ZM",
                    LastName = "eTutor",
                    UserName = adminEmail,
                    NormalizedEmail = adminEmail.ToUpper(),
                    NormalizedUserName = adminEmail.ToUpper(),
                    CreatedDate = DateTime.Now,
                    PersonalId = "000-0000000-4",
                    UpdatedDate = DateTime.Now,
                    IsActive = true,
                    AccessFailedCount = 0,
                    PasswordHash = hasher.HashPassword(new User(), "123456")
                };

                context.Add(user);
                context.SaveChanges();

                UserRole role = new UserRole {UserId = user.Id, RoleId = (int) RoleTypes.Admin};
                context.Add(role);
                context.SaveChanges();
            }

            if (tutor == null)
            {
                User user = new User
                {
                    Email = tutorEmail,
                    EmailConfirmed = true,
                    Gender = Gender.Male,
                    IsEmailValidated = true,
                    IsTemporaryPassword = false,
                    Name = "Tutor",
                    LastName = "eTutor",
                    SecurityStamp = "xATyuXy4vgUC9JZ833ja",
                    UserName = tutorEmail,
                    NormalizedEmail = tutorEmail.ToUpper(),
                    NormalizedUserName = tutorEmail.ToUpper(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    PersonalId = "000-0000000-2",
                    AccessFailedCount = 0,
                    IsActive = true,
                    PasswordHash = hasher.HashPassword(new User(), "123456"),
                };

                context.Add(user);
                context.SaveChanges();

                UserRole role = new UserRole {UserId = user.Id, RoleId = (int) RoleTypes.Tutor};
                context.Add(role);
                context.SaveChanges();
            }

            if (student == null)
            {
                User user = new User
                {
                    Email = studentEmail,
                    EmailConfirmed = true,
                    Gender = Gender.Male,
                    IsEmailValidated = true,
                    IsTemporaryPassword = false,
                    Name = "Student",
                    LastName = "eTutor",
                    SecurityStamp = "V0kjQ4wR2Zi9Z2Z2mvwz",
                    UserName = studentEmail,
                    NormalizedEmail = studentEmail.ToUpper(),
                    NormalizedUserName = studentEmail.ToUpper(),
                    CreatedDate = DateTime.Now,
                    BirthDate = new DateTime(2002, 2, 23),
                    IsActive = true,
                    UpdatedDate = DateTime.Now,
                    PersonalId = "000-0000000-0",
                    AccessFailedCount = 0,
                    PasswordHash = hasher.HashPassword(new User(), "123456"),
                };

                context.Add(user);
                context.SaveChanges();

                UserRole role = new UserRole {UserId = user.Id, RoleId = (int) RoleTypes.Student};
                context.Add(role);
                context.SaveChanges();
            }

            if (parent == null)
            {
                User user = new User
                {
                    Email = parentEmail,
                    EmailConfirmed = true,
                    Gender = Gender.Male,
                    IsEmailValidated = true,
                    IsTemporaryPassword = false,
                    Name = "Student",
                    LastName = "eTutor",
                    SecurityStamp = "gJJTwcxqoSgZ66zH3vPM",
                    UserName = parentEmail,
                    NormalizedEmail = parentEmail.ToUpper(),
                    NormalizedUserName = parentEmail.ToUpper(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsActive = true,
                    PersonalId = "000-0000000-1",
                    AccessFailedCount = 0,
                    PasswordHash = hasher.HashPassword(new User(), "123456"),
                };

                context.Add(user);
                context.SaveChanges();

                UserRole role = new UserRole {UserId = user.Id, RoleId = (int) RoleTypes.Parent};
                context.Add(role);
                context.SaveChanges();
            }

            student = context.Users
                .Include(s => s.UserRoles)
                .Include(s => s.Parents)
                .FirstOrDefault(u => u.Email == studentEmail);
            if (student != null && !student.Parents.Any()) 
            {
                parent = context.Users
                    .Include(u => u.UserRoles)
                    .Include(u => u.Students)
                    .FirstOrDefault(u => u.Email == parentEmail);
                
                var relationship = new ParentStudent
                {
                    ParentId = parent.Id,
                    StudentId = student.Id,
                    Relationship = ParentRelationship.Father
                };
                context.Add(relationship);
                context.SaveChanges();
            }
        }
    }
}

