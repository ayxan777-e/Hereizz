using System.Text.Json;
using Application.Shared.Responses;
using FluentValidation;

namespace API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        BaseResponse response;

        switch (ex)
        {
            case ValidationException validationException:
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var errors = validationException.Errors
                        .Select(e => e.ErrorMessage)
                        .Distinct()
                        .ToList();

                    response = BaseResponse.Fail(
                        "Validation failed",
                        errors,
                        ErrorType.BadRequest
                    );
                    break;
                }

            case KeyNotFoundException keyNotFoundException:
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;

                    response = BaseResponse.Fail(
                        "Resource not found",
                        new List<string> { keyNotFoundException.Message },
                        ErrorType.NotFound
                    );
                    break;
                }

            case UnauthorizedAccessException unauthorizedAccessException:
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    response = BaseResponse.Fail(
                        "Unauthorized",
                        new List<string> { unauthorizedAccessException.Message },
                        ErrorType.Unauthorized
                    );
                    break;
                }

            case ArgumentException argumentException:
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    response = BaseResponse.Fail(
                        "Bad request",
                        new List<string> { argumentException.Message },
                        ErrorType.BadRequest
                    );
                    break;
                }

            case InvalidOperationException invalidOperationException:
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    response = BaseResponse.Fail(
                        "Operation is not valid",
                        new List<string> { invalidOperationException.Message },
                        ErrorType.BadRequest
                    );
                    break;
                }

            default:
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    response = BaseResponse.Fail(
                        "Internal server error",
                        new List<string> { "An unexpected error occurred." },
                        ErrorType.ServerError
                    );
                    break;
                }
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}