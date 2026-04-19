using API.Controllers.Common;
using Application.Commands.Orders.Checkout;
using Application.Commands.Orders.DeleteOrder;
using Application.Commands.Orders.UpdateOrderStatus;
using Application.DTOs.Orders;
using Application.Queries.Orders.GetAllOrders;
using Application.Queries.Orders.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Constants;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : BaseApiController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetOrdersQuery query)
    {
        var result = await _mediator.Send(query);
        return HandleResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return HandleResponse(result);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateOrderStatusRequest request,
        CancellationToken ct)
    {
        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            Status = request.Status
        };

        var result = await _mediator.Send(command, ct);
        return HandleResponse(result);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CancellationToken ct)
    {
        var result = await _mediator.Send(new CheckoutCommand(), ct);
        return HandleResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteOrderCommand(id), ct);
        return HandleResponse(result);
    }
}