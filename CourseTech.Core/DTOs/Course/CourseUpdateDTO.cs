using CourseTech.Shared.Enums;

namespace CourseTech.Core.DTOs.Course
{
    public record CourseUpdateDTO(Guid Id, string Title, string? Description, string ImageUrl, string VideoUrl, CourseLevel Level, CourseLanguage Language, decimal Price, TimeSpan Duration, bool IsPublished, DateTime? PublishedAt, Guid InstructorId, Guid CategoryId);
}