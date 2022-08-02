using Cars.COMMON.DTOs;
using FluentValidation;

namespace Cars.BLL.Validators
{
    public class CarUpdateDTOValidator : AbstractValidator<CarUpdateDTO>
    {
        public CarUpdateDTOValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
            
            RuleFor(p => p.Price)
                .GreaterThan(0);

            RuleFor(p => p.Mileage)
                .GreaterThan(0);
        }
    }
}