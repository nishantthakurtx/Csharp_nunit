using CourseTech.Shared.Enums;

namespace CourseTech.Core.DTOs.Course
{
    public record CourseCreateDTO(string Title, string? Description, string ImageUrl, CourseLevel Level, CourseLanguage Language, decimal Price, TimeSpan Duration, Guid InstructorId, Guid CategoryId);

}
