using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Enums;
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
                : "your selected products";

            string subject;
            string content;

            switch (order.Status)
            {
                case OrderStatus.Confirmed:
                    subject = "Your order has been confirmed";
                    content = $"Your order for <b>{productsText}</b> has been successfully confirmed.";
                    break;

                case OrderStatus.Processing:
                    subject = "Your order is being processed";
                    content = $"We are currently preparing your order for <b>{productsText}</b>.";
                    break;

                case OrderStatus.Shipped:
                    subject = "Your order is on the way";
                    content = $"Your order for <b>{productsText}</b> has been shipped and is on the way.";
                    break;

                case OrderStatus.Delivered:
                    subject = "Your order has been delivered";
                    content = $"Your order for <b>{productsText}</b> has been delivered successfully.";
                    break;

                case OrderStatus.Cancelled:
                    subject = "Your order has been cancelled";
                    content = $"Your order for <b>{productsText}</b> has been cancelled.";
                    break;

                default:
                    subject = "Order status updated";
                    content = $"Your order status for <b>{productsText}</b> has been updated to <b>{order.Status}</b>.";
                    break;
            }

            var body = $"""
                <div style="font-family: Arial, sans-serif; padding: 20px;">
                    <h2 style="color: #2c3e50;">Hereizzz</h2>
                    
                    <h3 style="color: #34495e;">{subject}</h3>
                    
                    <p style="font-size: 15px;">
                        {content}
                    </p>

                    <hr style="margin:20px 0;" />

                    <p style="font-size: 13px; color: gray;">
                        If you have any questions, feel free to contact us.
                    </p>

                    <p style="font-size: 13px; color: gray;">
                        Best regards,<br/>
                        <b>Hereizzz Team</b>
                    </p>
                </div>
                """;

            await _emailService.SendEmailAsync(order.User.Email, subject, body, cancellationToken);
        }

        return BaseResponse<bool>.Ok(true, "Order status updated");
    }
}