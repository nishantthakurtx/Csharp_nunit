using CourseTech.Core.Models;
using CourseTech.Core.Repositories;
using System.Linq.Expressions;

public class FakeCourseRepository : ICourseRepository
{
    public List<Course> Courses = new();

    public Task<Course?> GetByIdAsync(Guid id) =>
        Task.FromResult(Courses.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Course>> GetPublishedCoursesAsync() =>
        Task.FromResult(Courses.Where(c => c.IsPublished).AsEnumerable());

    public Task<IEnumerable<Course>> GetAllCoursesWithDetailsAsync() =>
        Task.FromResult(Courses.AsEnumerable());

    public Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId) =>
        Task.FromResult(Courses.Where(c => c.InstructorId == instructorId).AsEnumerable());

    public Task<IEnumerable<Course>> GetCoursesByCategoryAsync(Guid categoryId) =>
        Task.FromResult(Courses.Where(c => c.CategoryId == categoryId).AsEnumerable());

    public Task<Course?> GetCourseWithDetailsAsync(Guid courseId) =>
        Task.FromResult(Courses.FirstOrDefault(c => c.Id == courseId));

    public Task<IEnumerable<Course>> GetAllAsync() =>
        Task.FromResult(Courses.AsEnumerable());

    public Task InsertAsync(Course entity)
    {
        Courses.Add(entity);
        return Task.CompletedTask;
    }

    public void InsertFake(Course course)
    {
        Courses.Add(course); 
    }

    public Course Update(Course entity)
    {
        var existing = Courses.FirstOrDefault(c => c.Id == entity.Id);
        if (existing != null)
        {
            Courses.Remove(existing);
        }
        Courses.Add(entity);
        return entity;
    }

    public void SoftDelete(Course entity)
    {
        entity.MarkAsDeleted();
    }

    public IQueryable<Course> Where(Expression<Func<Course, bool>> predicate)
    {
        return Courses.AsQueryable().Where(predicate);
    }

}
