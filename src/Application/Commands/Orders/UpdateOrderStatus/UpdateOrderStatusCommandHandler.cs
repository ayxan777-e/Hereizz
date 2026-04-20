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
    private readonly INotificationService _notificationService;
    private readonly IEmailTemplateService _emailTemplateService;

    public UpdateOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        IEmailService emailService,
        INotificationService notificationService,
        IEmailTemplateService emailTemplateService)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
        _notificationService = notificationService;
        _emailTemplateService = emailTemplateService;
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

            var emailTemplate = _emailTemplateService.BuildOrderStatusUpdatedTemplate(productsText, order.Status);

            await _emailService.SendEmailAsync(
                order.User.Email,
                emailTemplate.Subject,
                emailTemplate.Body,
                cancellationToken);

            await _notificationService.CreateAsync(
                order.UserId,
                emailTemplate.Subject,
                $"Status updated for {productsText}: {order.Status}",
                NotificationType.OrderStatusUpdated,
                cancellationToken);
        }

        return BaseResponse<bool>.Ok(true, "Order status updated");
    }
}