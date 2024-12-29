using CourseTech.Shared.Enums;

namespace CourseTech.Core.DTOs.Course
{
    public record CourseDTO(Guid Id, string Title, string? Description, string ImageUrl, CourseLevel Level, CourseLanguage Language, decimal Price, TimeSpan Duration, DateTime? PublishedAt, string InstructorName, string CategoryName);
}