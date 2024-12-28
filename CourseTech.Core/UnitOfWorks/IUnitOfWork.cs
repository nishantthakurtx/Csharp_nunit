using CourseTech.Core.Models;
using CourseTech.Core.Repositories;

namespace CourseTech.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        void SaveChanges();
    }
}