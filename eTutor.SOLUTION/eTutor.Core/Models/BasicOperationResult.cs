using eTutor.Core.Contracts;

namespace eTutor.Core.Models
{
    public class BasicOperationResult<T>: IOperationResult<T>
    {
        public BasicOperationResult(T entity, bool success, Error message)
        {
            Entity = entity;
            Success = success;
            Message = message;
        }
        
        public T Entity { get; }
        
        public bool Success { get; }
        
        public Error Message { get; }

        public static IOperationResult<T> Ok() 
            => new BasicOperationResult<T>(default, true, null);
        

        public static IOperationResult<T> Ok(T entity)
            => new BasicOperationResult<T>(entity, true, null);
        
        public static IOperationResult<T> Fail(string message) 
            => new BasicOperationResult<T>(default, false, new Error{Code = 400, Message = message, ReasonPhrase = "Error"});
    }
}