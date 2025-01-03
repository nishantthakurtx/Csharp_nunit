namespace CourseTech.Core.DTOs.Payment.Stripe
{
    public record CreateCardResource
    (
        string Name,
        string Number,
        string ExpiryYear,
        string ExpiryMonth,
        string Cvc
    );
}