namespace CourseTech.Core.Models
{
    public class Payment
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public AppUser AppUser { get; private set; } = null!;

        public decimal TotalAmount { get; private set; }
        public string PaymentProvider { get; private set; }
        public string TransactionId { get; private set; }
        public bool IsSuccessFull { get; private set; }
        public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;


        public Payment(Guid userId, AppUser appUser, decimal totalAmount, string paymentProvider, string transactionId)
        {
            UserId = userId;
            AppUser = appUser;
            TotalAmount = totalAmount;
            PaymentProvider = paymentProvider;
            TransactionId = transactionId;
        }

        public void MarkAsSuccessful()
        {
            IsSuccessFull = true;
        }

        public void MarkAsFailed()
        {
            IsSuccessFull = false;
        }
    }
}
