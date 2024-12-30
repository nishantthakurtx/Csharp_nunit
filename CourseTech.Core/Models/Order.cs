namespace CourseTech.Core.Models
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public AppUser AppUser { get; private set; } = null!;

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        
        private decimal _totalPrice;
        public decimal TotalPrice => _totalPrice;
        
        public Order(Guid userId, AppUser user)
        {
            UserId = userId;
            AppUser = user;
        }

        public void AddOrderItem(Course course)
        {
            if (_orderItems.Any(x => x.CourseId == course.Id))
                throw new InvalidOperationException("Course is already in the order.");

            _orderItems.Add(new OrderItem(course));
            RecalculateTotalPrice();
        }
        private void RecalculateTotalPrice()
        {
            _totalPrice = _orderItems.Sum(x => x.Price);
        }
    }
}
