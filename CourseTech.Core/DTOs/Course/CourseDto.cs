using CourseTech.Core.Models;
using CourseTech.Shared.Enums;

namespace CourseTech.Core.DTOs.Course
{
    public record CourseDto(Guid Id,string Title, string? Description, string ImageUrl, string VideoUrl, Category Category, CourseLanguage Language, decimal Price, CourseLevel Level, TimeSpan Duration, string Instructor, bool isPublished);
}