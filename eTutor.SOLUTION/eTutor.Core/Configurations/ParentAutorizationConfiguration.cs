using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class ParentAutorizationConfiguration : IEntityTypeConfiguration<ParentAutorization>
    {
        public void Configure(EntityTypeBuilder<ParentAutorization> builder)
        {
            builder.HasOne(p => p.Parent)
                .WithMany(u => u.Autorizations);

        }
    }
}