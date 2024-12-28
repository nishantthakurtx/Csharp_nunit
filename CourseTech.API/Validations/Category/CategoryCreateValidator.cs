using CourseTech.Core.DTOs.Category;
using FluentValidation;

namespace CourseTech.API.Validations.Category
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        }
    }
}