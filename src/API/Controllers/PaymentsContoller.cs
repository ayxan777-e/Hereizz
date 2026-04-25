using API.Controllers.Common;
using Application.Commands.Payments.Pay;
using Application.Queries.Payments.GetAllPayments;
using Application.Queries.Payments.GetMyPayments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PaymentsController : BaseApiController
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id}/pay")]
    public async Task<IActionResult> Pay(int id)
    {
        var result = await _mediator.Send(new PayCommand
        {
            PaymentId = id
        });

        return HandleResponse(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyPayments()
    {
        var result = await _mediator.Send(new GetMyPaymentsQuery());
        return HandleResponse(result);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPaymentsQuery());
        return HandleResponse(result);
    }
}