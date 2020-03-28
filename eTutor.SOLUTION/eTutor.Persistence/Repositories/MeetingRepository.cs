using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence.Repositories
{
    public sealed class MeetingRepository : BaseRepository<Meeting>, IMeetingRepository
    {
        private readonly ETutorContext _context;
        public MeetingRepository(ETutorContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ISet<Meeting>> GetAllMeetingsOfParentStudents(int parentId)
        {

            var result = await _context.Meetings
                .Include(m => m.Student)
                .ThenInclude(s => s.Parents)
                .Include(m => m.Subject)
                .Include(m => m.Tutor)
                .Where(m => m.Student.Parents.Any(p => p.ParentId == parentId))
                .ToListAsync();
            

            return result.ToHashSet();
        }

        public async Task<Meeting> GetMeetingForParent(int meetingId, int parentId)
        {
            var result = await _context.Meetings
                .Include(m => m.Subject)
                .Include(m => m.Tutor)
                .Include(m => m.Student)
                .ThenInclude(st => st.Parents)
                .FirstOrDefaultAsync(m => m.Id == meetingId &&
                                          m.Student.Parents.Any(p => p.ParentId == parentId));

                return result;
        }

        public async Task<Meeting> GetLastCompleteMeetingForUser(int userId, RoleTypes role)
        {
            var meetings = await _context.Meetings
                .Include(m => m.Subject)
                .Include(m => m.Tutor)
                .Include(m => m.Student)
                .Where(m => (m.StudentId == userId || m.TutorId == userId)
                            && m.Status == MeetingStatus.Complete
                            && m.RealEndedDateTime != null)
                .OrderByDescending(m => m.RealEndedDateTime)
                .ToListAsync();
                
            var meeting = meetings.FirstOrDefault();
            
            return meeting;
        }
    }


}