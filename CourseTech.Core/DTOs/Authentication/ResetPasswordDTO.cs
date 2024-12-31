namespace CourseTech.Core.DTOs.Authentication
{
    public record ResetPasswordDTO(string Email, string Token, string NewPassword);
}