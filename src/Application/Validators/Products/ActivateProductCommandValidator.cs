using Application.Commands.Product.ActivateProduct;
using FluentValidation;

namespace Application.Validators.Product;

public class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
{
    public ActivateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}