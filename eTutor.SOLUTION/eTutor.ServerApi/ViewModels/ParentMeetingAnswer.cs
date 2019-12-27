using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class ParentMeetingAnswer
    {
        public ParentAuthorizationStatus StatusAnswer { get; set; }
        
        public string Reason { get; set; }
    }
}