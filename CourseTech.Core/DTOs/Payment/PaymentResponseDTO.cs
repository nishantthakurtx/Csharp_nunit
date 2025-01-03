namespace CourseTech.Core.DTOs.Payment
{
    public record PaymentResponseDTO
    (
        Guid Id,
        decimal TotalAmount,
        string PaymentProvider,
        string TransactionId,
        string Status,
        DateTime PaymentDate,
        bool IsSuccessFull,
        string Message
    );
}