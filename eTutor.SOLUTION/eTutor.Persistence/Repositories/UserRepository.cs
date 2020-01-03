using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ETutorContext _context;
        public UserRepository(ETutorContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ISet<User>> GetAllParentsForStudent(int studentId)
        {
            var users = await Set.Include(u => u.Students)
                .Where(u => u.Students.Any(s => s.StudentId == studentId))
                .ToListAsync();
            
            return users.ToHashSet();
        }

        public async Task<ISet<RoleTypes>> GetRolesForUser(int userId)
        {
            var roles = await _context
                .UserRoles.Where(ur => ur.UserId == userId)
                .Select(ur => (RoleTypes) Enum.ToObject(typeof(RoleTypes), ur.RoleId)).ToListAsync();

            return roles.ToHashSet();
        }

        public async Task<ISet<User>> GetAllStudentsForParent(int parentId)
        {
            var users = await Set.Include(u => u.Parents)
                .Where(u => u.Parents.Any(p => p.ParentId == parentId)).ToListAsync();

            return users.ToHashSet();
        }
    }
}