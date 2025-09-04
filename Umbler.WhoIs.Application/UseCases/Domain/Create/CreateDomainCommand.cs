using MediatR;

namespace Umbler.WhoIs.Application.UseCases.Domain.Create;

/// <summary>
/// Command to create or refresh a domain record with resolved data.
/// </summary>
/// <param name="DomainName">Fully-qualified domain name.</param>
/// <param name="IpAddress">Resolved IPv4 address.</param>
/// <param name="WhoIs">Raw WHOIS payload for the domain.</param>
/// <param name="Ttl">DNS TTL (seconds).</param>
/// <param name="HostedAt">Organization that owns the IP (WHOIS).</param>
public sealed record CreateDomainCommand(
    string DomainName,
    string IpAddress,
    string WhoIs,
    int Ttl,
    string HostedAt
) : IRequest<CreateDomainCommandResponse>;