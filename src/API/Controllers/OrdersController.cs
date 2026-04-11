using Application.Commands.Orders.Checkout;
using Application.Commands.Orders.DeleteOrder;
using Application.Commands.Orders.UpdateOrderStatus;
using Application.DTOs.Orders;
using Application.Queries.Orders.GetAllOrders;
using Application.Queries.Orders.GetOrderById;
using Application.Shared.Responses;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
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
        return StatusCode((int)result.ErrorType, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<OrderDetailsDto>>> GetOrder(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return StatusCode((int)result.ErrorType, result);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<BaseResponse<bool>>> UpdateStatus(
        int id,
        UpdateOrderStatusRequest request,
        CancellationToken ct)
    {
        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            Status = request.Status
        };

        var result = await _mediator.Send(command, ct);
        return StatusCode((int)result.ErrorType, result);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<BaseResponse<int>>> Checkout(CancellationToken ct)
    {
        var result = await _mediator.Send(new CheckoutCommand(), ct);
        return StatusCode((int)result.ErrorType, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteOrder(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteOrderCommand(id), ct);
        return StatusCode((int)result.ErrorType, result);
    }
}