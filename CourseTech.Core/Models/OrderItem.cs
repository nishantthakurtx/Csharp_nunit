namespace CourseTech.Core.Models
{
    public class OrderItem
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public Guid CourseId { get; private set; }
        public Course Course { get; private set; } = null!;
        public decimal Price { get; private set; }

        public OrderItem(Course course)
        {
            CourseId = course.Id;
            Course = course;
            Price = course.Price;
        }
    }
}