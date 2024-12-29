using AutoMapper;
using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.Models;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseTech.Service.Services
{
    public class UserService(UserManager<AppUser> userManager, IMapper mapper) : IUserService
    {
        public async Task<ServiceResult<AppUserDTO>> CreateAsync(AppUserDTO createUserDto, string role)
        {
            var user = new AppUser
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                UserName = createUserDto.Email
            };

            var createResult = await userManager.CreateAsync(user, createUserDto.Password);
            if (!createResult.Succeeded)
                return ServiceResult<AppUserDTO>.Fail(createResult.Errors.Select(e => e.Description).ToList());

            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                return ServiceResult<AppUserDTO>.Fail(roleResult.Errors.Select(e => e.Description).ToList());

            var userDto = mapper.Map<AppUserDTO>(user);
            return ServiceResult<AppUserDTO>.Success(userDto);
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return ServiceResult.Fail($"User ({id}) not found.");

            user.MarkAsDeleted();
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return ServiceResult.Fail(result.Errors.Select(e => e.Description).ToList());

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<IEnumerable<AppUserDTO>>> GetAllAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = mapper.Map<IEnumerable<AppUserDTO>>(users);
            return ServiceResult<IEnumerable<AppUserDTO>>.Success(userDtos);
        }

        public async Task<ServiceResult<AppUserDTO>> GetByIdAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return ServiceResult<AppUserDTO>.Fail($"User ({id}) not found.");

            return ServiceResult<AppUserDTO>.Success(mapper.Map<AppUserDTO>(user));
        }

        public async Task<ServiceResult<IEnumerable<string>>> GetRolesAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ServiceResult<IEnumerable<string>>.Fail($"User ({userId}) not found.");

            var roles = await userManager.GetRolesAsync(user);
            return ServiceResult<IEnumerable<string>>.Success(roles);
        }

        public async Task<ServiceResult> UpdateAsync(AppUserDTO updateUserDto, Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return ServiceResult.Fail($"User ({id}) not found.");

            mapper.Map(updateUserDto, user);
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return ServiceResult.Fail(result.Errors.Select(e => e.Description).ToList());

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<IEnumerable<AppUserDTO>>> Where(Expression<Func<AppUser, bool>> predicate)
        {
            var users = await userManager.Users.Where(predicate).ToListAsync();
            if (users == null)
                return ServiceResult<IEnumerable<AppUserDTO>>.Success(Enumerable.Empty<AppUserDTO>());

            var userDtos = mapper.Map<IEnumerable<AppUserDTO>>(users);
            return ServiceResult<IEnumerable<AppUserDTO>>.Success(userDtos);
        }
    }
}