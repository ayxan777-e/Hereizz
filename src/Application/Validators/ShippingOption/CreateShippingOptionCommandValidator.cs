using Application.Commands.ShippingOption.CreateShippingOption;
using FluentValidation;

namespace Application.Validators.ShippingOption;

public class CreateShippingOptionCommandValidator : AbstractValidator<CreateShippingOptionCommand>
{
    public CreateShippingOptionCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();

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