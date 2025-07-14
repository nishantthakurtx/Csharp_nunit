using CourseTech.Core.DTOs.Course;
using System;
using System.Collections.Generic;

namespace CourseTech.Tests.Helpers
{
    public static class CourseTestData
    {
        public static CourseDTO GetSampleCourseDTO() =>
            new CourseDTO(
                TestConstants.SampleCourseId,
                "C# Basics",
                "Learn fundamentals of C#",
                "image.jpg",
                "video.mp4",
                "Beginner",
                "English",
                199,
                TimeSpan.FromHours(5),
                DateTime.UtcNow,
                "John Instructor",
                "Programming",
                DateTime.UtcNow
            );

        public static List<CourseDTO> GetCourseDTOList() =>
            new() { GetSampleCourseDTO() };

        public static CourseCreateDTO GetSampleCreateDTO() =>
            new("C# Basics", "Learn", "image.jpg", "video.mp4", "Beginner", "English", 199, TimeSpan.FromHours(5), Guid.NewGuid(), Guid.NewGuid());
    }
}
