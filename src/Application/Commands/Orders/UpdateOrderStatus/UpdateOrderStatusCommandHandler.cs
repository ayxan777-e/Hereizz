using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Orders.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, BaseResponse<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;

    public UpdateOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithUserAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return BaseResponse<bool>.Fail(
                "Order not found",
                new List<string> { "Order with given id does not exist" },
                ErrorType.NotFound
            );
        }

        if (!OrderStatusTransitionRule.CanTransition(order.Status, request.Status))
        {
            return BaseResponse<bool>.Fail(
                "Invalid status transition",
                new List<string> { $"Cannot change status from {order.Status} to {request.Status}" },
                ErrorType.BusinessRule
            );
        }

        order.Status = request.Status;

        await _orderRepository.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(order.User?.Email))
        {
            var productNames = order.Items
                .Select(x => x.ProductTitle)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var productsText = productNames.Any()
                ? string.Join(", ", productNames)
                : "Your selected products";

            var subject = "Order status updated";

            var body = $"""
                <h3>Your order status has been updated</h3>
                <p>Product Name(s): {productsText}</p>
                <p>New Status: {order.Status}</p>
                """;

            await _emailService.SendEmailAsync(order.User.Email, subject, body, cancellationToken);
        }

        return BaseResponse<bool>.Ok(true, "Order status updated");
    }
}