namespace CourseTech.Core.DTOs.Payment.Stripe
{
    public record CustomerResource
    (
        string CustomerId,
        string Email,
        string Name
    );
}
