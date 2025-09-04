using Microsoft.AspNetCore.Mvc;

namespace Umbler.WhoIs.WebApp.Common;

/// <summary>
/// Base API controller with helper response methods.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Returns HTTP 200 with the given payload.
    /// </summary>
    /// <param name="data">Response payload.</param>
    /// <typeparam name="T">Payload type.</typeparam>
    protected IActionResult Ok<T>(T data) =>
        base.Ok(data);

    /// <summary>
    /// Returns HTTP 201 with route info and a wrapped payload.
    /// </summary>
    /// <param name="routeName">Named route.</param>
    /// <param name="routeValues">Route values.</param>
    /// <param name="data">Response data.</param>
    /// <typeparam name="T">Payload type.</typeparam>
    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    /// <summary>
    /// Returns HTTP 400 with a message envelope.
    /// </summary>
    /// <param name="message">Error message.</param>
    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    /// <summary>
    /// Returns HTTP 404 with a message envelope.
    /// </summary>
    /// <param name="message">Optional not-found message.</param>
    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });
}