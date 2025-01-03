namespace CourseTech.Core.DTOs.Payment.Stripe
{
    public record CreateChargeResource
    (
        string Currency,
        long Amount,
        string CustomerId,
        string ReceiptEmail,
        string Description
    );
}