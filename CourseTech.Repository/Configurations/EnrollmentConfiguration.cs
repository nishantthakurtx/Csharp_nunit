using CourseTech.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.Repository.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => new { e.AppUserId, e.CourseId });

            // AppUser(Student) - Enrollment(Many-to-Many)
            builder.HasOne(e => e.AppUser)
                .WithMany(a => a.Enrollments)
                .HasForeignKey(e => e.AppUserId);

            // Course - Enrollment(Many-to-Many)
            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

        }
    }
}