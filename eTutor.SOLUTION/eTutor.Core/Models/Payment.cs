namespace eTutor.Core.Models
{
    public sealed class Payment : EntityBase
    {
        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public double PayedAmount { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}