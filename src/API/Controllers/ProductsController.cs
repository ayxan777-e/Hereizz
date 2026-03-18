using Application.DTOs.Product;
using Application.Queries.Products.GetProductById;
using Application.Queries.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductListItemResponse>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailResponse>> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}