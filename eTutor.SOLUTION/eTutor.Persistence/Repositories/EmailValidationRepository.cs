using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class EmailValidationRepository : BaseRepository<EmailValidation>, IEmailValidationRepository
    {
        public EmailValidationRepository(ETutorContext context) : base(context)
        {
        }
    }
}