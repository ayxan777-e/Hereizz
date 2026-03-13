using Application.Commands.Orders.CreateOrder;
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
}