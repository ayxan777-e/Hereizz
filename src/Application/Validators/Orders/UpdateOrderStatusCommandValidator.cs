using Application.Commands.Orders.UpdateOrderStatus;
using FluentValidation;

namespace Application.Validators.Orders;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("OrderId must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid order status.");
    }
}