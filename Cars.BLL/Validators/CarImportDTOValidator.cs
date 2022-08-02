using Cars.COMMON.DTOs;
using FluentValidation;

namespace Cars.BLL.Validators
{
    public class CarImportDTOValidator : AbstractValidator<CarImportDTO>
    {
        public CarImportDTOValidator()
        {
            RuleFor(p => p.Vin)
                .Length(17)
                .NotEmpty();

            RuleFor(p => p.Year)
                .GreaterThan(1900)
                .NotEmpty();

            RuleFor(p => p.Make)
                .NotEmpty();

            RuleFor(p => p.Model)
                .NotEmpty();

            RuleFor(p => p.PriceUnformatted)
                .GreaterThan(0);

            RuleFor(p => p.MileageUnformatted)
                .GreaterThan(0);
        }
    }
}