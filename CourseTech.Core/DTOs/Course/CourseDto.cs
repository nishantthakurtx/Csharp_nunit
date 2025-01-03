namespace CourseTech.Core.DTOs.Course
{
    public record CourseDTO
    (
        Guid Id, 
        string Title, 
        string? Description, 
        string ImageUrl, 
        string Level, 
        string Language, 
        decimal Price, 
        TimeSpan Duration, 
        DateTime? PublishedAt, 
        string InstructorName, 
        string CategoryName);
}