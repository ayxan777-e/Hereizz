using API.Controllers.Common;
using Application.Commands.Cart.AddItem;
using Application.Commands.Cart.ClearCart;
using Application.Commands.Cart.RemoveItem;
using Application.Commands.Cart.UpdateItemQuantity;
using Application.Queries.Cart.GetMyCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : BaseApiController
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
        return HandleResponse(result);
    }
    [HttpPost("add-items")]
    public async Task<IActionResult> AddItemToCart(AddItemToCartCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }
    [HttpPut("items")]
    public async Task<IActionResult> UpdateCartItemQuantity(
     [FromBody] UpdateCartItemQuantityCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }
    [HttpDelete("items")]
    public async Task<IActionResult> RemoveCartItem([FromBody] RemoveCartItemCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var result = await _mediator.Send(new ClearCartCommand());
        return HandleResponse(result);
    }
}