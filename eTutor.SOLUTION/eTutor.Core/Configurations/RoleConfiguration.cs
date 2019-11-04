using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasIndex(r => r.Name).IsUnique();

            IEnumerable<RoleTypes> enumValues = Enum
                .GetValues(typeof(RoleTypes)).Cast<RoleTypes>();

            var roles = enumValues.Select(r => new Role
            {
                Id = (int) r,
                Name = r.GetEnumValueDescription(),
                NormalizedName = r.GetEnumValueDescription()
            });

            builder.HasData(roles);
        }
    }
}
