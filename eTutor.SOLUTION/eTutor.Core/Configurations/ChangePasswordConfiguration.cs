using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public sealed class ChangePasswordConfiguration : IEntityTypeConfiguration<ChangePassword>
    {
        public void Configure(EntityTypeBuilder<ChangePassword> builder)
        {
            builder.HasIndex(b => b.ChangeRequestId).IsUnique();
            builder.Property(b => b.ChangeRequestId).IsRequired();

            builder.HasOne(b => b.User)
                .WithMany(b => b.ChangeRequests)
                .HasForeignKey(b => b.UserId);
        }
    }
}
