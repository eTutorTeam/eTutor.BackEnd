namespace eTutor.ServerApi.ViewModels
{
    public sealed class UserLoginRequest
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}