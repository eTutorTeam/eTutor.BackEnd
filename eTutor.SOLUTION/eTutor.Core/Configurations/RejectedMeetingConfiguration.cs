using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public sealed class RejectedMeetingConfiguration : IEntityTypeConfiguration<RejectedMeeting>
    {
        public void Configure(EntityTypeBuilder<RejectedMeeting> builder)
        {
            builder.HasOne(r => r.Meeting)
                .WithMany()
                .HasForeignKey(r => r.MeetingId);

            builder.HasIndex(r => r.MeetingId);

            builder.HasOne(r => r.Tutor)
                .WithMany()
                .HasForeignKey(r => r.TutorId);

            builder.HasIndex(r => r.TutorId);
        }
    }
}