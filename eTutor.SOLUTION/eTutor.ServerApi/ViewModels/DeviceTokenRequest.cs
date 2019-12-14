using System.Collections.Generic;

namespace eTutor.ServerApi.ViewModels
{
    public class DeviceTokenRequest
    {
        public int UserId { get; set; }
        
        public string Platform { get; set; }
        
        public string FcmToken { get; set; }
    }
}