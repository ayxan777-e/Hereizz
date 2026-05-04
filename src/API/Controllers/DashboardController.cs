using API.Controllers.Common;
using Application.Queries.Dashboard.GetAdminDashboard;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DashboardController : BaseApiController
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("admin")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAdminDashboard([FromQuery] int? year)
    {
        var result = await _mediator.Send(new GetAdminDashboardQuery
        {
            Year = year ?? DateTime.UtcNow.Year
        });

        return HandleResponse(result);
    }
}