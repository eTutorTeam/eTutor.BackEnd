using System.Collections.Generic;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;

namespace eTutor.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ISet<User>> GetAllParentsForStudent(int studentId);

        Task<ISet<RoleTypes>> GetRolesForUser(int userId);
        
        Task<ISet<User>> GetAllStudentsForParent(int parentId);
    }
}