using API.Controllers.Common;
using Application.Commands.ShippingOption.ActivateShippingOption;
using Application.Commands.ShippingOption.CreateShippingOption;
using Application.Commands.ShippingOption.DeleteShippingOption;
using Application.Commands.ShippingOption.UpdateShippingOption;
using Application.DTOs.ShippingOption;
using Application.Queries.ShippingOptions.GetShippingOptionById;
using Application.Queries.ShippingOptions.GetShippingOptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShippingOptionsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ShippingOptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var response = await _mediator.Send(new GetShippingOptionsQuery(false), ct);
        return HandleResponse(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetShippingOptionByIdQuery(id, false), ct);
        return HandleResponse(response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken ct)
    {
        var response = await _mediator.Send(new GetShippingOptionsQuery(true), ct);
        return HandleResponse(response);
    }

    [HttpGet("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByIdAdmin(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetShippingOptionByIdQuery(id, true), ct);
        return HandleResponse(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateShippingOptionRequest request, CancellationToken ct)
    {
        var response = await _mediator.Send(new CreateShippingOptionCommand(request), ct);
        return HandleResponse(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShippingOptionRequest request, CancellationToken ct)
    {
        var response = await _mediator.Send(new UpdateShippingOptionCommand(id, request), ct);
        return HandleResponse(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new DeleteShippingOptionCommand(id), ct);
        return HandleResponse(response);
    }

    [HttpPatch("{id}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var response = await _mediator.Send(new ActivateShippingOptionCommand(id), ct);
        return HandleResponse(response);
    }
}