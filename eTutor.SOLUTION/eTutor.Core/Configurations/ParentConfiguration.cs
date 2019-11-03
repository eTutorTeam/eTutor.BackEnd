using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.HasOne(p => p.User)
                .WithOne(u => u.Parent);

            builder.HasMany(p => p.Students)
                .WithOne(ps => ps.Parent);
        }
    }
}