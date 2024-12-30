namespace CourseTech.Core.DTOs.Basket
{
    public record BasketItemDTO(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        decimal Price
    );
}