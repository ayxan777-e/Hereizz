using FluentValidation;

namespace Application.Commands.Cart.RemoveItem;

public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .GreaterThan(0)
            .WithMessage("Cart item id must be greater than 0.");
    }
}