using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class UserConiguration : IEntityTypeConfiguration<User> 
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // u => Represents a single user
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.PersonalId)
                .IsUnique();
        }
    }
}
