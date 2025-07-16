using CourseTech.Core.DTOs.AppUser;

namespace CourseTech.Tests.Helpers
{
    public static class UserTestData
    {
        public static AppUserDTO GetUserDto() => new(
            Guid.NewGuid(),
            "Test",
            "User",
            "test.user@example.com",
            "1234567890"
        );

        public static AppUserWithPasswordDTO GetCreateUserDto() => new(
            Guid.NewGuid(),
            "Test",
            "User",
            "test.user@example.com",
            "Password@123",
            "1234567890"
        );

        public static AppUserWithNamesDTO GetUserWithNameDto() => new(
            Guid.NewGuid(),
            "Test User"
        );
    }
}
