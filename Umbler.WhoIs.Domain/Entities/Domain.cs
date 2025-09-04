using Umbler.WhoIs.Common.Validations;
using Umbler.WhoIs.Domain.Common;
using Umbler.WhoIs.Domain.Validations;

namespace Umbler.WhoIs.Domain.Entities;

/// <summary>
/// Aggregate root representing a resolved domain snapshot.
/// </summary>
public class Domain : BaseEntity
{
    /// <summary>
    /// Fully-qualified domain name.
    /// </summary>
    public string DomainName { get; init; } = string.Empty;

    /// <summary>
    /// Resolved IPv4 address.
    /// </summary>
    public string IpAddress { get; init; } = string.Empty;

    /// <summary>
    /// Raw WHOIS content for the domain.
    /// </summary>
    public string WhoIs { get; init; } = string.Empty;

    /// <summary>
    /// TTL (seconds) used to determine data freshness.
    /// </summary>
    public int Ttl { get; init; } = -1;

    /// <summary>
    /// Organization that owns/hosts the IP (from WHOIS).
    /// </summary>
    public string HostedAt { get; init; } = string.Empty;

    /// <summary>
    /// Last update timestamp (UTC), if any.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }


    /// <summary>
    /// Validates the entity via DomainValidator and returns a summarized result.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new DomainValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}