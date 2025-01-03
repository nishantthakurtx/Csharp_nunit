namespace CourseTech.Core.DTOs.Payment
{
    public record PaymentDTO
    (
        string CustomerId,
        string PaymentId,
        string ReceiptEmail,
        string Description,
        long Amount,
        string Currency
    );
}