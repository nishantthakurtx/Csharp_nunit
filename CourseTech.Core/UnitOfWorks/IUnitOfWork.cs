using CourseTech.Core.Repositories;

namespace CourseTech.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICourseRepository Course { get; }
        IEnrollmentRepository Enrollment { get; }
        Task SaveChangesAsync();
        void SaveChanges();
    }
}