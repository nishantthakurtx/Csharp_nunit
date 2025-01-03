namespace CourseTech.Core.DTOs.Payment
{
    public record PaymentRequestDTO
    (
        Guid UserId,
        Guid BasketId,
        decimal TotalAmount,
        string PaymentProvider,
        string Currency
    );
}