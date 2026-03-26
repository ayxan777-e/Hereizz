using Application.Commands.Cart.AddItem;
using Application.Queries.Cart.GetMyCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyCart()
    {
        var result = await _mediator.Send(new GetMyCartQuery());
        return Ok(result);
    }
    [HttpPost("add-items")]
    public async Task<IActionResult> AddItemToCart(AddItemToCartCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}