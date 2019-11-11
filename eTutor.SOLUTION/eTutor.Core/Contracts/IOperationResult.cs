using eTutor.Core.Models;

namespace eTutor.Core.Contracts
{
    public interface IOperationResult<T>
    {
        T Entity { get; }
        bool Success { get; }
        Error Message { get; }
    }
}