using Application.Commands.Orders.UpdateOrderStatus;
using FluentValidation;

namespace Application.Validators.Orders;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .Must(status => Enum.IsDefined(status))
            .WithMessage("Invalid order status");
    }
}