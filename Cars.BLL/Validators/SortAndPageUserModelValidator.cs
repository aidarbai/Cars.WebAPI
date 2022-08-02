using Cars.COMMON.ViewModels.Users;
using FluentValidation;

namespace Cars.BLL.Validators
{
    public class SortAndPageUserModelValidator : AbstractValidator<SortAndPageUserModel>
    {
        public SortAndPageUserModelValidator()
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