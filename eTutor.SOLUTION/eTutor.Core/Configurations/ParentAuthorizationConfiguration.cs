using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class ParentAuthorizationConfiguration : IEntityTypeConfiguration<ParentAuthorization>
    {
        public void Configure(EntityTypeBuilder<ParentAuthorization> builder)
        {
            builder.HasOne(p => p.Parent)
                .WithMany(u => u.Autorizations);

        }
    }
}