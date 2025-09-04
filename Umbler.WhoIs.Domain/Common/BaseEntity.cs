using Umbler.WhoIs.Common.Validations;

namespace Umbler.WhoIs.Domain.Common;

/// <summary>
/// Base entity with identity and common behaviors.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Entity identifier (UUID).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creation timestamp (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Runs the registered validator for this entity type.
    /// </summary>
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    /// <summary>
    /// Compares by Id for stable ordering.
    /// </summary>
    /// <param name="other">Other entity.</param>
    /// <returns>Comparison result.</returns>
    public int CompareTo(BaseEntity? other)
    {
        return other == null ? 1 : other.Id.CompareTo(Id);
    }
}