using AutoMapper;

namespace CourseTech.Tests.Helpers.Mapping
{
    public static class TestMapper
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TestMappingProfile>();
            });

            return config.CreateMapper();
        }
    }
}
