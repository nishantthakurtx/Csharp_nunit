using CourseTech.Shared.Enums;

namespace CourseTech.Core.Models
{
    public class Basket : BaseEntity
    {
        public Guid UserId { get; set; } = default!;
        public AppUser AppUser { get; set; } = null!;

        public BasketStatus Status { get; private set; } = BasketStatus.Active;

        public ICollection<BasketItem> BasketItems { get; private set; } = new List<BasketItem>();

        public decimal TotalPrice => BasketItems.Sum(item => item.Price);


        public void AddCourse(Course course)
        {
            if (Status != BasketStatus.Active)
                throw new InvalidOperationException("Only active baskets can be modified.");

            if (BasketItems.Any(item => item.CourseId == course.Id))
                throw new InvalidOperationException("Course already exists in the basket.");

            BasketItems.Add(new BasketItem(course));
        }

        public void RemoveCourse(Guid courseId)
        {
            var item = BasketItems.FirstOrDefault(i => i.CourseId == courseId);
            if (item == null)
                throw new InvalidOperationException("Course not found in the basket.");

            BasketItems.Remove(item);
        }

        public void ClearBasket()
        {
            if (Status != BasketStatus.Active)
                throw new InvalidOperationException("Only active baskets can be cleared.");

            BasketItems.Clear();
        }

        public void CompleteBasket()
        {
            if (!BasketItems.Any())
                throw new InvalidOperationException("Cannot complete an empty basket.");

            Status = BasketStatus.Completed;
        }
    }
}