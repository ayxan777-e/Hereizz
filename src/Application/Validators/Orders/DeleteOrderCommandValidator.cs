using Application.Commands.Orders.DeleteOrder;
using FluentValidation;

namespace Application.Validators.Orders;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("OrderId must be greater than 0.");
    }
}