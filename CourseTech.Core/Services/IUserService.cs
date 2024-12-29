using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.Models;
using CourseTech.Shared;
using System.Linq.Expressions;

namespace CourseTech.Core.Services
{
    public interface IUserService
    {
        Task<ServiceResult<AppUserDTO>> CreateAsync(AppUserDTO createUserDto, string role);
        Task<ServiceResult<AppUserDTO>> GetByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<AppUserDTO>>> GetAllAsync();
        Task<ServiceResult<IEnumerable<AppUserDTO>>> Where(Expression<Func<AppUser, bool>> predicate);
        Task<ServiceResult> UpdateAsync(AppUserDTO updateUserDto, Guid id);
        Task<ServiceResult> DeleteAsync(Guid id);

        Task<ServiceResult<IEnumerable<string>>> GetRolesAsync(Guid userId);
    }
}
