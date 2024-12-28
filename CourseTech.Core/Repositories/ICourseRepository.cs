using CourseTech.Core.Models;

namespace CourseTech.Core.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetPublishedCoursesAsync();
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId);
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(string categoryName);

    }
}