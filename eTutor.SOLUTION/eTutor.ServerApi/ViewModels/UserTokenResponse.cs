namespace eTutor.ServerApi.ViewModels
{
    public sealed class UserTokenResponse
    {
        public int uId { get; set; }
        public string Token { get; set; }
        
        public int[] Roles { get; set; }
        
        public string UserName { get; set; }
        
        public string FullName { get; set; }
        
        public string ProfileImageUrl { get; set; }
        
        public string Email { get; set; }
    }
}