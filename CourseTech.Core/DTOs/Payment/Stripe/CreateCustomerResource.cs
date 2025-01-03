namespace CourseTech.Core.DTOs.Payment.Stripe
{
    public record CreateCustomerResource
    (
        string Email,
        string Name,
        CreateCardResource Card
    );
}
