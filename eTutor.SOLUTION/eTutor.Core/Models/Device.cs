namespace eTutor.Core.Models
{
    public sealed class Device : EntityBase
    {
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public string Platform { get; set; }
        
        public string FcmToken { get; set; }
    }
}