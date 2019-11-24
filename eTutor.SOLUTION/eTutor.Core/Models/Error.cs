namespace eTutor.Core.Models
{
    public sealed class Error
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string ReasonPhrase { get; set; }
    }
}