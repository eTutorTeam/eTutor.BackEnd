using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var result = await _context.ParentStudents
                .Include(ps => ps.Student)
                    .ThenInclude(s => s.StudentMeetings)
                        .ThenInclude(m => m.Subject)
                .Include(ps => ps.Student)
                    .ThenInclude(s => s.StudentMeetings)
                        .ThenInclude(m => m.Tutor)
                .Include(ps => ps.Student)
                    .ThenInclude(s => s.StudentMeetings)
                        .ThenInclude(m => m.Student)
                .SelectMany(ps => ps.Student.StudentMeetings)
                .ToListAsync();

            return result.ToHashSet();
        }
    }


}
