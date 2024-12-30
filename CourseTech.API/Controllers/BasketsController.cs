using CourseTech.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(IBasketService service) : CustomBaseController
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBasket(Guid userId)
        {
            var result = await service.GetActiveBasketAsync(userId);
            return CreateActionResult(result);
        }

        [HttpPost("add-course")]
        public async Task<IActionResult> AddCourseToBasket([FromQuery] Guid userId, [FromQuery] Guid courseId)
        {
            var result = await service.AddCourseToBasketAsync(userId, courseId);
            return CreateActionResult(result);
        }

        [HttpDelete("remove-course")]
        public async Task<IActionResult> RemoveCourseFromBasket([FromQuery] Guid userId, [FromQuery] Guid courseId)
        {
            var result = await service.RemoveCourseFromBasketAsync(userId, courseId);
            return CreateActionResult(result);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearBasket([FromQuery] Guid userId)
        {
            var result = await service.ClearBasketAsync(userId);
            return CreateActionResult(result);
        }

        [HttpPatch("complete")]
        public async Task<IActionResult> CompleteBasket([FromQuery] Guid userId)
        {
            var result = await service.CompleteBasketAsync(userId);
            return CreateActionResult(result);
        }
    }
}