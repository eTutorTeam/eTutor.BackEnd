namespace eTutor.ServerApi.ViewModels
{
    public sealed class ForgetPasswordChangeRequest
    {
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
    }
}