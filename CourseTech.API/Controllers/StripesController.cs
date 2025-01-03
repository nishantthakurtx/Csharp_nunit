using CourseTech.Core.DTOs.Payment.Stripe;
using CourseTech.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripesController(IStripeService service) : CustomBaseController
    {
        [HttpPost("customer")]
        public async Task<ActionResult<CustomerResource>> CreateCustomer([FromBody] CreateCustomerResource resource,
        CancellationToken cancellationToken)
        {
            var response = await service.CreateCustomerAsync(resource, cancellationToken);
            return Ok(response);
        }

        [HttpPost("charge")]
        public async Task<ActionResult<ChargeResource>> CreateCharge([FromBody] CreateChargeResource resource, CancellationToken cancellationToken)
        {
            var response = await service.CreateChargeAsync(resource, cancellationToken);
            return Ok(response);
        }
    }
}