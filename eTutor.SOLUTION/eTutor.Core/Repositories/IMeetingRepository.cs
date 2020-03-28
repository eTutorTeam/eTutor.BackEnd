using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;

namespace eTutor.Core.Repositories
{
    public interface IMeetingRepository: IGenericRepository<Meeting>
    {
        Task<ISet<Meeting>> GetAllMeetingsOfParentStudents(int parentId);

        Task<Meeting> GetMeetingForParent(int meetingId, int parentId);
        
        Task<Meeting> GetLastCompleteMeetingForUser(int userId, RoleTypes role);
    }
}
