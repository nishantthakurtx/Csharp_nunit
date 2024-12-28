using CourseTech.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseTech.Repository.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();

            // AppUser - Enrollment (One-to-Many)
            builder.HasMany(a => a.Enrollments)
                   .WithOne(e => e.AppUser)
                   .HasForeignKey(e => e.AppUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // AppUser - CreatedCourses (One-to-Many)
            builder.HasMany(a => a.CreatedCourses)
                   .WithOne(c => c.Instructor)
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}