using Cars.COMMON.ViewModels.Cars;
using FluentValidation;

namespace Cars.BLL.Validators
{
    public class SortAndPageCarModelValidator : AbstractValidator<SortAndPageCarModel>
    {
        public SortAndPageCarModelValidator()
        {
            RuleFor(p => p.SortBy)
                .MaximumLength(30);

            RuleFor(p => p.Order)
                .IsInEnum();

            RuleFor(p => p.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(20);

            RuleFor(p => p.PageNumber)
                .GreaterThan(0);
        }
    }
}