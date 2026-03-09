using Application.DTOs.Product;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductListItemResponse>>> GetAll(CancellationToken ct)
    {
        var products = await _productService.GetAllAsync(ct);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailResponse>> GetById(int id, CancellationToken ct)
    {
        var product = await _productService.GetByIdAsync(id, ct);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
}