namespace Umbler.WhoIs.Application.UseCases.Domain.Create;

/// <summary>
/// Response DTO returned after create/refresh operations.
/// </summary>
/// <param name="DomainName">Domain name.</param>
/// <param name="IpAddress">Resolved IP.</param>
/// <param name="HostedAt">Owning organization (WHOIS).</param>
public sealed record CreateDomainCommandResponse(string DomainName, string IpAddress, string HostedAt);