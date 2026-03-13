using Domain.Enums;

namespace Application.DTOs.Orders;

public class UpdateOrderStatusRequest
{
    public OrderStatus Status { get; set; }
}