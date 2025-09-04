using FluentValidation;
using Umbler.WhoIs.Common.Validations.Utils;

namespace Umbler.WhoIs.Application.UseCases.Domain.Create;

/// <summary>
/// FluentValidation rules for CreateDomainCommand.
/// </summary>
public sealed class CreateDomainCommandValidator : AbstractValidator<CreateDomainCommand>
{
    /// <summary>
    /// Adds domain, IP, WHOIS, host, and TTL validation rules.
    /// </summary>
    public CreateDomainCommandValidator()
    {
        RuleFor(x => x.DomainName)
            .NotEmpty()
            .Must(DnsValidator.IsValidDomainName)
            .WithMessage("Domain Name is invalid");
        RuleFor(x => x.IpAddress)
            .NotEmpty()
            .Must(DnsValidator.IsValidIPv4Address)
            .WithMessage("Ip Address is invalid");
        RuleFor(x => x.WhoIs).NotEmpty();
        RuleFor(x => x.HostedAt).NotEmpty();
        RuleFor(x => x.Ttl).GreaterThanOrEqualTo(0);
    }
}