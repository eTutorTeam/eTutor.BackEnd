namespace eTutor.ServerApi.ViewModels
{
    public sealed class UserTokenResponse
    {
        public string Token { get; set; }
        
        public int[] Roles { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
    }
}