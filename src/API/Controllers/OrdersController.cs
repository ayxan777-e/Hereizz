using Application.Commands.Orders.CreateOrder;
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