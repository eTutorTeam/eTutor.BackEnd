using System.IO;
using System.Threading.Tasks;

namespace eTutor.Core.Contracts
{
    public interface IFileService
    {
        Task<string> UploadStreamToBucketServer(Stream file, string fileName);

        Task DeleteFileFromBucketServer(string fileName);
    }
}