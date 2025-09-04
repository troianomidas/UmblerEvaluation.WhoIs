using System.Text.Json;
using FluentValidation;
using Umbler.WhoIs.Common.Validations;
using Umbler.WhoIs.WebApp.Common;

namespace Umbler.WhoIs.WebApp.Middleware;

/// <summary>
/// Middleware that converts exceptions into standardized JSON responses.
/// </summary>
/// <param name="next">Next pipeline delegate.</param>
public class ValidationExceptionMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Executes the middleware and handles known exceptions.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleInternalErrorExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Writes a 400 response for FluentValidation errors.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="exception">Validation exception.</param>
    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new ApiResponse
        {
            Success = false,
            Message = "Validation Failed",
            Errors = exception.Errors
                .Select(error => (ValidationErrorDetail)error)
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }

    /// <summary>
    /// Writes a 409 response for conflict errors.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="exception">Thrown exception.</param>
    private static Task HandleConflictExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status409Conflict;

        var response = new ApiResponseError
        {
            Type = "Conflict",
            Error = "Resource Already Exists",
            Detail = exception.Message
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }

    /// <summary>
    /// Writes a 500 response for unexpected errors.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="exception">Thrown exception.</param>
    private static Task HandleInternalErrorExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new ApiResponseError
        {
            Type = "Error",
            Error = "Internal Error",
            Detail = exception.Message
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}