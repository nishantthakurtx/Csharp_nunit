using CourseTech.Core.Models;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using CourseTech.Tests.Helpers;

namespace CourseTech.Tests.Mocks
{
    public class MockUserManager : Mock<UserManager<AppUser>>
    {
        public AppUser FakeUser { get; } = new()
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            UserName = "john.doe@example.com"
        };

        public MockUserManager() : base(
            Mock.Of<IUserStore<AppUser>>(),
            null, null, null, null, null, null, null, null)
        {
            // Basic setups
            Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(FakeUser);

            Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(FakeUser);

            Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);

            Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<AppUser>()))
                .ReturnsAsync("test-reset-token");

            Setup(x => x.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            Setup(x => x.GetUsersInRoleAsync("Student"))
                .ReturnsAsync(new List<AppUser> { FakeUser });

            Setup(x => x.GetUsersInRoleAsync("Instructor"))
                .ReturnsAsync(new List<AppUser> { FakeUser });


            var userList = new List<AppUser> { FakeUser }
            .AsQueryable()
            .BuildMockDbSet(); 

            Setup(x => x.Users).Returns(userList.Object);

        }
    }
}
