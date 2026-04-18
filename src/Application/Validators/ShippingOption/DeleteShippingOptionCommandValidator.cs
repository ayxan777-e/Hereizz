using Application.Commands.ShippingOption.DeleteShippingOption;
using FluentValidation;

namespace Application.Validators.ShippingOption;

public class DeleteShippingOptionCommandValidator : AbstractValidator<DeleteShippingOptionCommand>
{
    public DeleteShippingOptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}