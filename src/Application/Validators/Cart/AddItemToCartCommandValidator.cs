using Application.Commands.Cart.AddItem;
using FluentValidation;

namespace Application.Validators.Cart;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.ShippingOptionId)
            .GreaterThan(0)
            .WithMessage("ShippingOptionId must be greater than 0.");
    }
}