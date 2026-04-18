using Application.Commands.ShippingOption.UpdateShippingOption;
using FluentValidation;

namespace Application.Validators.ShippingOption;

public class UpdateShippingOptionCommandValidator : AbstractValidator<UpdateShippingOptionCommand>
{
    public UpdateShippingOptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Request.EstimatedMinDays)
            .GreaterThan(0);

        RuleFor(x => x.Request.EstimatedMaxDays)
            .GreaterThanOrEqualTo(x => x.Request.EstimatedMinDays);

        RuleFor(x => x.Request.PricePerKg)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Request.FixedFee)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Request.OriginCountry)
            .IsInEnum();

        RuleFor(x => x.Request.DestinationCountry)
            .IsInEnum();

        RuleFor(x => x.Request.TransportType)
            .IsInEnum();
    }
}