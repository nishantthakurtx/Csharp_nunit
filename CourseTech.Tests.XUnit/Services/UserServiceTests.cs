using AutoMapper;
using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.Models;
using CourseTech.Service.Services;
using CourseTech.Tests.Mocks;
using CourseTech.Tests.Helpers;
using CourseTech.Tests.Helpers.Mapping;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CourseTech.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _service;
        private readonly MockUserManager _userManager;
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            _userManager = new MockUserManager();
            _mapper = TestMapper.CreateMapper();
            _service = new UserService(_userManager.Object, _mapper);
        }

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ReturnsSuccess()
        {
            var result = await _service.GetByIdAsync(_userManager.FakeUser.Id);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
              .ReturnsAsync((AppUser?)null!);


            var result = await _service.GetByIdAsync(Guid.NewGuid());
            Assert.True(result.IsFailure);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_ReturnsUsers()
        {
            var result = await _service.GetAllAsync();
            Assert.True(result.IsSuccess);
        }

        #endregion

        #region GetInstructorsAsync

        [Fact]
        public async Task GetInstructorsAsync_ReturnsSuccess()
        {
            var result = await _service.GetInstructorsAsync();
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetInstructorsAsync_ReturnsFail_WhenEmpty()
        {
            _userManager.Setup(x => x.GetUsersInRoleAsync("Instructor"))
                        .ReturnsAsync([]);

            var result = await _service.GetInstructorsAsync();
            Assert.True(result.IsFailure);
        }

        #endregion

        #region GetStudentsAsync

        [Fact]
        public async Task GetStudentsAsync_ReturnsSuccess()
        {
            var result = await _service.GetStudentsAsync();
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetStudentsAsync_ReturnsFail_WhenEmpty()
        {
            _userManager.Setup(x => x.GetUsersInRoleAsync("Student"))
                        .ReturnsAsync([]);

            var result = await _service.GetStudentsAsync();
            Assert.True(result.IsFailure);
        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ReturnsSuccess()
        {
            var dto = UserTestData.GetCreateUserDto();

            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Student"))
                        .ReturnsAsync(IdentityResult.Success);

            var result = await _service.CreateAsync(dto);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFail_WhenCreateFails()
        {
            var dto = UserTestData.GetCreateUserDto();

            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Create failed" }));


            var result = await _service.CreateAsync(dto);
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task CreateAsync_ReturnsFail_WhenRoleAssignFails()
        {
            var dto = UserTestData.GetCreateUserDto();

            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
             .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Student"))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Role assign failed" }));

            var result = await _service.CreateAsync(dto);
            Assert.True(result.IsFailure);
        }

        #endregion

        #region AssignRoleAsync

        [Fact]
        public async Task AssignRoleAsync_ReturnsSuccess()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                        .ReturnsAsync(_userManager.FakeUser);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Student"))
                        .ReturnsAsync(IdentityResult.Success);

            var result = await _service.AssignRoleAsync(_userManager.FakeUser.Id, "Student");
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AssignRoleAsync_ReturnsFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                        .ReturnsAsync((AppUser)null!);

            var result = await _service.AssignRoleAsync(Guid.NewGuid(), "Student");
            Assert.True(result.IsFail);
        }

        [Fact]
        public async Task AssignRoleAsync_ReturnsFail_WhenRoleFails()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                        .ReturnsAsync(_userManager.FakeUser);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Add role failed" }));

            var result = await _service.AssignRoleAsync(_userManager.FakeUser.Id, "Student");
            Assert.True(result.IsFail);
        }

        #endregion

        #region ResetPasswordAsync

        [Fact]
        public async Task ResetPasswordAsync_ReturnsSuccess()
        {
            var result = await _service.ResetPasswordAsync(_userManager.FakeUser.Email!, "NewPass@123");
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ResetPasswordAsync_ReturnsFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync((AppUser)null!);

            var result = await _service.ResetPasswordAsync("not@found.com", "NewPass@123");
            Assert.True(result.IsFail);
        }

        [Fact]
        public async Task ResetPasswordAsync_ReturnsFail_WhenResetFails()
        {
            _userManager.Setup(x => x.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Reset failed" }));

            var result = await _service.ResetPasswordAsync(_userManager.FakeUser.Email!, "NewPass@123");
            Assert.True(result.IsFail);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess()
        {
            var dto = UserTestData.GetUserDto() with { Id = _userManager.FakeUser.Id };

            _userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                        .ReturnsAsync(IdentityResult.Success);

            var result = await _service.UpdateAsync(dto);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                        .ReturnsAsync((AppUser)null!);

            var dto = UserTestData.GetUserDto();
            var result = await _service.UpdateAsync(dto);
            Assert.True(result.IsFail);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenUpdateFails()
        {
            var dto = UserTestData.GetUserDto() with { Id = _userManager.FakeUser.Id };

            _userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

            var result = await _service.UpdateAsync(dto);
            Assert.True(result.IsFail);
        }

        #endregion

        #region SoftDeleteAsync

        [Fact]
        public async Task SoftDeleteAsync_ReturnsSuccess()
        {
            _userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                        .ReturnsAsync(IdentityResult.Success);

            var result = await _service.SoftDeleteAsync(_userManager.FakeUser.Id);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SoftDeleteAsync_ReturnsFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                        .ReturnsAsync((AppUser)null!);

            var result = await _service.SoftDeleteAsync(Guid.NewGuid());
            Assert.True(result.IsFail);
        }

        [Fact]
        public async Task SoftDeleteAsync_ReturnsFail_WhenUpdateFails()
        {
            _userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Soft delete failed" }));

            var result = await _service.SoftDeleteAsync(_userManager.FakeUser.Id);
            Assert.True(result.IsFail);
        }

        #endregion
    }
}
