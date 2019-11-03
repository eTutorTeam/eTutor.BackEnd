using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasOne(s => s.User)
                .WithOne(u => u.Student);

            builder
                .HasMany(s => s.Parents)
                .WithOne(ps => ps.Student);
        }
    }
}