using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : CustomBaseController
    {
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CreateActionResult(await userService.GetByIdAsync(id));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await userService.GetAllAsync());
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Where([FromQuery] string emailContains)
        {
            return CreateActionResult(await userService.Where(user => user.Email!.Contains(emailContains)));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUserDto createUserDto, string role)
        {
            return CreateActionResult(await userService.CreateAsync(createUserDto, role));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, AppUserDto updateUserDto)
        {
            return CreateActionResult(await userService.UpdateAsync(updateUserDto, id));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CreateActionResult(await userService.DeleteAsync(id));
        }
    }
}
