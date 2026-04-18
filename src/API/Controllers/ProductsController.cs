using API.Controllers.Common;
using Application.Commands.Product.ActivateProduct;
using Application.Commands.Product.DeleteProduct;
using Application.Commands.Product.ImportProduct;
using Application.Commands.Product.UpdateProduct;
using Application.DTOs.Product;
using Application.Queries.Products.GetProductById;
using Application.Queries.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var response = await _mediator.Send(new GetProductsQuery(false), ct);
        return HandleResponse(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetProductByIdQuery(id, false), ct);
        return HandleResponse(response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken ct)
    {
        var response = await _mediator.Send(new GetProductsQuery(true), ct);
        return HandleResponse(response);
    }

    [HttpGet("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByIdAdmin(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetProductByIdQuery(id, true), ct);
        return HandleResponse(response);
    }

    [HttpPost("import")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Import([FromBody] ImportProductRequest request, CancellationToken ct)
    {
        var response = await _mediator.Send(new ImportProductCommand(request), ct);
        return HandleResponse(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        var response = await _mediator.Send(new UpdateProductCommand(id, request), ct);
        return HandleResponse(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new DeleteProductCommand(id), ct);
        return HandleResponse(response);
    }

    [HttpPatch("{id}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new ActivateProductCommand(id), ct);
        return HandleResponse(response);
    }
}