using CourseTech.Core.DTOs.Payment.Stripe;
using CourseTech.Shared;

namespace CourseTech.Core.Services
{
    public interface IStripeService : IPaymentService
    {
        Task<ServiceResult<CustomerResource>> CreateCustomerAsync(CreateCustomerResource resource, CancellationToken cancellationToken);
        Task<ServiceResult<ChargeResource>> CreateChargeAsync(CreateChargeResource resource, CancellationToken cancellationToken);
    }
}