using Application.Commands.Orders.CreateOrder;
using Application.Commands.Orders.DeleteOrder;
using Application.Commands.Orders.UpdateOrderStatus;
using Application.DTOs.Orders;
using Application.Queries.Orders.GetOrderById;
using Application.Queries.Orders.GetOrders;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<List<OrderListItemDto>>>> GetOrders(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrdersQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<OrderDetailsDto>>> GetOrder(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPatch("{id}/status")]
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

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<int>>> CreateOrder(
        CreateOrderCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteOrder(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteOrderCommand(id), ct);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}