using CourseTech.Core.DTOs.Payment.Stripe;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Stripe;
using System.Net;

namespace CourseTech.Service.Services
{
    public class StripeService(TokenService tokenService, CustomerService customerService, ChargeService chargeService) : IStripeService
    {
        public async Task<ServiceResult<ChargeResource>> CreateChargeAsync(CreateChargeResource resource, CancellationToken cancellationToken)
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Currency = resource.Currency,
                Amount = resource.Amount,
                ReceiptEmail = resource.ReceiptEmail,
                Customer = resource.CustomerId,
                Description = resource.Description
            };

            var charge = await chargeService.CreateAsync(chargeOptions, null, cancellationToken);
            if (charge == null)
                return ServiceResult<ChargeResource>.Fail("Charge failed.");

            var chargeResource = new ChargeResource(charge.Id, charge.Currency, charge.Amount, charge.CustomerId, charge.ReceiptEmail, charge.Description);

            return ServiceResult<ChargeResource>.Success(chargeResource, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<CustomerResource>> CreateCustomerAsync(CreateCustomerResource resource, CancellationToken cancellationToken)
        {
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = resource.Card.Name,
                    Number = resource.Card.Number,
                    ExpYear = resource.Card.ExpiryYear,
                    ExpMonth = resource.Card.ExpiryMonth,
                    Cvc = resource.Card.Cvc
                }
            };
            var token = await tokenService.CreateAsync(tokenOptions, null, cancellationToken);

            if (token == null)
                return ServiceResult<CustomerResource>.Fail("Token creation failed.");

            var customerOptions = new CustomerCreateOptions
            {
                Email = resource.Email,
                Name = resource.Name,
                Source = token.Id
            };
            var customer = await customerService.CreateAsync(customerOptions, null, cancellationToken);

            if (customer == null)
                return ServiceResult<CustomerResource>.Fail("Customer creation failed.");

            var customerResource = new CustomerResource(customer.Id, customer.Email, customer.Name);
            return ServiceResult<CustomerResource>.Success(customerResource);
        }
    }
}