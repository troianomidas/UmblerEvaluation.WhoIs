using FluentValidation.Results;

namespace Umbler.WhoIs.Common.Validations;

/// <summary>
/// Aggregated validation result with a boolean flag and a list of errors.
/// </summary>
public class ValidationResultDetail
{
    /// <summary>
    /// Indicates whether the validation succeeded.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Collection of validation errors (empty when valid).
    /// </summary>
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];

    public ValidationResultDetail()
    {
    }

    /// <summary>
    /// Adapts a FluentValidation ValidationResult to a ValidationResultDetail.
    /// </summary>
    public ValidationResultDetail(ValidationResult validationResult)
    {
        IsValid = validationResult.IsValid;
        Errors = validationResult.Errors.Select(o => (ValidationErrorDetail)o);
    }
}