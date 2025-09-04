using Umbler.WhoIs.Common.Validations;

namespace Umbler.WhoIs.WebApp.Common;

/// <summary>
/// Standard API response envelope (without data).
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool Success { get; set; }
   
    /// <summary>
    /// Human-readable message for the client.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional validation errors (if any).
    /// </summary>
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];
}