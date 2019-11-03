using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
    {
        public void Configure(EntityTypeBuilder<Tutor> builder)
        {
            builder.HasOne(t => t.User)
                .WithOne(u => u.Tutor);
        }
    }
}