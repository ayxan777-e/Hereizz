using Application.Commands.Orders.CreateOrder;
using FluentValidation;

namespace Application.Validators.Orders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0.");

        RuleFor(x => x.ShippingOptionId)
            .GreaterThan(0)
            .WithMessage("ShippingOptionId must be greater than 0.");
    }
}