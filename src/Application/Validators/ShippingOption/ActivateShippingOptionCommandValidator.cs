using Application.Commands.ShippingOption.ActivateShippingOption;
using FluentValidation;

namespace Application.Validators.ShippingOption;

public class ActivateShippingOptionCommandValidator : AbstractValidator<ActivateShippingOptionCommand>
{
    public ActivateShippingOptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}