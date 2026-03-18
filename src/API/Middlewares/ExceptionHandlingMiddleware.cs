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
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();

            var response = BaseResponse.Fail("Validation failed", errors);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = BaseResponse.Fail(
                "Internal server error",
                new List<string> { "An unexpected error occurred." }
            );

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}