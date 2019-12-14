using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class EmailValidationConfiguration : IEntityTypeConfiguration<EmailValidation>
    {
        public void Configure(EntityTypeBuilder<EmailValidation> builder)
        {
            builder.HasOne(e => e.User)
                .WithOne(u => u.EmailValidation);

            builder.HasIndex(e => e.UserId).IsUnique();
            builder.HasIndex(e => e.ValidationToken).IsUnique();
        }
    }
}