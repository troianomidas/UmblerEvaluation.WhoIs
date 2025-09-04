using FluentValidation.Results;

namespace Umbler.WhoIs.Common.Validations;

/// <summary>
/// Lightweight, transport-friendly validation error detail.
/// </summary>
public class ValidationErrorDetail
{
    /// <summary>
    /// Machine-readable error code (if any).
    /// </summary>
    public string Error { get; init; } = string.Empty;

    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public string Detail { get; init; } = string.Empty;

    /// <summary>
    /// Maps FluentValidation's ValidationFailure to ValidationErrorDetail.
    /// </summary>
    public static explicit operator ValidationErrorDetail(ValidationFailure validationFailure)
    {
        return new ValidationErrorDetail
        {
            Detail = validationFailure.ErrorMessage,
            Error = validationFailure.ErrorCode
        };
    }
}