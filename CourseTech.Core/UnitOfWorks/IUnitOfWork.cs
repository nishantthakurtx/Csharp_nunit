using CourseTech.Core.Repositories;

namespace CourseTech.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        Task SaveChangesAsync();
        void SaveChanges();
    }
}