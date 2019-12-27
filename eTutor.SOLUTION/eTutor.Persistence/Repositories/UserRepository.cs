using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ETutorContext context) : base(context)
        {
        }

        public async Task<ISet<User>> GetAllParentsForStudent(int studentId)
        {
            var users = await Set.Include(u => u.Students)
                .Where(u => u.Students.Any(s => s.StudentId == studentId))
                .ToListAsync();
            
            return users.ToHashSet();
        }
    }
}