using Cars.BLL.ViewModels;
using FluentValidation;

namespace Cars.BLL.Validators
{
    public class LoginVmValidator : AbstractValidator<LoginVm>

    {
        public LoginVmValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(p => p.Password)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
