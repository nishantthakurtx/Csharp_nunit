using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.Models;
using CourseTech.Shared;
using System.Linq.Expressions;

namespace CourseTech.Core.Services
{
    public interface IUserService
    {
        Task<ServiceResult<AppUserDto>> CreateAsync(AppUserDto createUserDto, string role);
        Task<ServiceResult<AppUserDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<AppUserDto>>> GetAllAsync();
        Task<ServiceResult<IEnumerable<AppUserDto>>> Where(Expression<Func<AppUser, bool>> predicate);
        Task<ServiceResult> UpdateAsync(AppUserDto updateUserDto, Guid id);
        Task<ServiceResult> DeleteAsync(Guid id);

        Task<ServiceResult<IEnumerable<string>>> GetRolesAsync(Guid userId);
    }
}
