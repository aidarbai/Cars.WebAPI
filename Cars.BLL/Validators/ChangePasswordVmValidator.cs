using Cars.BLL.ViewModels;
using FluentValidation;

namespace Cars.BLL.Validators
{
    public class ChangePasswordVmValidator : AbstractValidator<ChangePasswordVm>

    {
        public ChangePasswordVmValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(p => p.OldPassword)
                .NotEmpty()
                .MaximumLength(30);
            
            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
