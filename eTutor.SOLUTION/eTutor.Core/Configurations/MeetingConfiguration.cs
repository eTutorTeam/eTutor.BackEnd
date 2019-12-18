using eTutor.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTutor.Core.Configurations
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.HasOne(b => b.Student)
                .WithMany(u => u.StudentMeetings)
                .HasForeignKey(b => b.StudentId);

            builder
                .HasOne(b => b.Subject)
                .WithMany(s => s.Meetings)
                .HasForeignKey(b => b.SubjectId);

            builder.HasOne(b => b.Tutor)
                .WithMany(u => u.TutorMeetings)
                .HasForeignKey(b => b.TutorId);
        }
    }
}