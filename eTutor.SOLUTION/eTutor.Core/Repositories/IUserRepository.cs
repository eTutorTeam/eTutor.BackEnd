using System.Collections.Generic;
using System.Threading.Tasks;
using eTutor.Core.Models;

namespace eTutor.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ISet<User>> GetAllParentsForStudent(int studentId);
    }
}