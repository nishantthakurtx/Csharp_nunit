using CourseTech.Core.Models;
using CourseTech.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Repository.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
        {
            return await _entities
                .Where(c => c.IsPublished && !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId)
        {
            return await _entities
                .Where(c => c.InstructorId == instructorId && !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(string categoryName)
        {
            return await _entities
                .Where(c => c.Category.Name == categoryName && !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}