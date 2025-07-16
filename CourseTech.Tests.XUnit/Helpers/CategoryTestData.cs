using CourseTech.Core.DTOs.Category;
using System;
using System.Collections.Generic;

namespace CourseTech.Tests.Helpers
{
    public static class CategoryTestData
    {
        public static CategoryDTO GetSampleCategoryDTO()
        {
            return new CategoryDTO(TestConstants.SampleCategoryId, "Sample Category");
        }

        public static List<CategoryDTO> GetSampleCategoryList()
        {
            return new List<CategoryDTO>
            {
                new CategoryDTO(TestConstants.SampleCategoryId, "Sample Category 1"),
                new CategoryDTO(Guid.NewGuid(), "Sample Category 2")
            };
        }
    }
}
