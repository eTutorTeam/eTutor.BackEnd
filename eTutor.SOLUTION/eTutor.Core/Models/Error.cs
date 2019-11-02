namespace eTutor.Core.Models
{
    public sealed class Error
    {
        public int Code { get; set; }

        public string Description { get; set; }

        public string ReasonPhrase { get; set; }
    }
}