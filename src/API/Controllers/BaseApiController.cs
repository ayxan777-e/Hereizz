using Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Common;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResponse(BaseResponse response)
    {
        if (response.Success)
            return Ok(response);

        return response.ErrorType switch
        {
            ErrorType.NotFound => NotFound(response),
            ErrorType.Unauthorized => Unauthorized(response),
            ErrorType.Conflict => Conflict(response),
            ErrorType.ServerError => StatusCode(StatusCodes.Status500InternalServerError, response),
            _ => BadRequest(response)
        };
    }

    protected IActionResult HandleResponse<T>(BaseResponse<T> response)
    {
        if (response.Success)
            return Ok(response);

        return response.ErrorType switch
        {
            ErrorType.NotFound => NotFound(response),
            ErrorType.Unauthorized => Unauthorized(response),
            ErrorType.Conflict => Conflict(response),
            ErrorType.ServerError => StatusCode(StatusCodes.Status500InternalServerError, response),
            _ => BadRequest(response)
        };
    }
}