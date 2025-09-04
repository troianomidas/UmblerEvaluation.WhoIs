namespace Umbler.WhoIs.WebApp.Common;

/// <summary>
/// API response envelope with a typed data payload.
/// </summary>
/// <typeparam name="T">Payload type.</typeparam>
public class ApiResponseWithData<T> : ApiResponse
{
    /// <summary>
    /// Data returned by the operation.
    /// </summary>
    public T? Data { get; set; }
}