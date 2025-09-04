namespace Umbler.WhoIs.WebApp.Common;

/// <summary>
/// Error payload used for non-validation failures.
/// </summary>
public class ApiResponseError
{
    /// <summary>
    /// Error category/type (e.g., Conflict, Error).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Short error code or label.
    /// </summary>
    public string Error { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed error description.
    /// </summary>
    public string Detail { get; set; } = string.Empty;
}