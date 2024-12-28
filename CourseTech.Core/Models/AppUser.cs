using Microsoft.AspNetCore.Identity;

namespace CourseTech.Core.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Course> CreatedCourses { get; set; } = new List<Course>();

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            Update();
        }

        public void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}