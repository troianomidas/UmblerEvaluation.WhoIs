using FluentValidation;

namespace Umbler.WhoIs.Domain.Validations;

/// <summary>
/// FluentValidation rules for the Domain aggregate.
/// </summary>
public class DomainValidator : AbstractValidator<Umbler.WhoIs.Domain.Entities.Domain>
{
    /// <summary>
    /// Ensures required fields are present and consistent.
    /// </summary>
    public DomainValidator()
    {
        RuleFor(domain => domain.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(domain => domain.DomainName)
            .NotNull()
            .NotEmpty();

        RuleFor(domain => domain.IpAddress)
            .NotNull()
            .NotEmpty();

        RuleFor(domain => domain.HostedAt)
            .NotNull()
            .NotEmpty();

        RuleFor(domain => domain.WhoIs)
            .NotNull()
            .NotEmpty();

        RuleFor(domain => domain.Ttl)
            .NotNull();
    }
}