using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(ETutorContext context) : base(context)
        {
        }
    }
}