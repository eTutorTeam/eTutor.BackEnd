using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class ParentStudentConfiguration : IEntityTypeConfiguration<ParentStudent>
    {
        public void Configure(EntityTypeBuilder<ParentStudent> builder)
        {
            builder.HasOne(ps => ps.Parent)
                .WithMany(u => u.Students);

            builder.HasOne(ps => ps.Student)
                .WithMany(u => u.Parents);
        }
    }
}