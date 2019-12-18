using System.Threading.Tasks;
using eTutor.Core.Models;

namespace eTutor.Core.Contracts
{
    public interface INotificationService
    {
        Task SendNotificationToUser(User user, string message, string subject = "eTutor");
    }
}