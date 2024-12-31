namespace CourseTech.Core.Models
{
    public class BasketItem
    {
        public Guid BasketId { get; set; }
        public Basket Basket { get; set; } = null!;

        public Guid CourseId { get; private set; }
        public Course Course { get; private set; } = null!;

        public BasketItem()
        {
        }

        public BasketItem(Course course)
        {
            CourseId = course.Id;
            Course = course;
        }
        public decimal Price => Course.Price;
    }
}