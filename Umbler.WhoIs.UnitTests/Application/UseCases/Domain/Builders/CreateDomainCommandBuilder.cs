using Umbler.WhoIs.Application.UseCases.Domain.Create;

namespace Umbler.WhoIs.UnitTests.Application.UseCases.Domain.Builders;

/// <summary>
/// Fluent builder for <see cref="CreateDomainCommand"/> used in unit tests.
/// </summary>
public sealed class CreateDomainCommandBuilder
{
    private string _domainName = "ns254.umbler.com";
    private string _ipAddress = "177.55.66.99";
    private string _whoIs = "json";
    private int _ttl = 30;
    private string _hostedAt = "Umbler";

    /// <summary>
    /// Builds the command with the current state.
    /// </summary>
    public CreateDomainCommand Build() =>
        new(_domainName, _ipAddress, _whoIs, _ttl, _hostedAt);

    /// <summary>
    /// Sets the domain name.
    /// </summary>
    private CreateDomainCommandBuilder WithDomain(string domainName)
    {
        _domainName = domainName;
        return this;
    }

    /// <summary>
    /// Sets the IP address.
    /// </summary>
    private CreateDomainCommandBuilder WithIp(string ipAddress)
    {
        _ipAddress = ipAddress;
        return this;
    }

    /// <summary>
    /// Sets the raw WHOIS content.
    /// </summary>
    private CreateDomainCommandBuilder WithWhoIs(string whoIs)
    {
        _whoIs = whoIs;
        return this;
    }

    /// <summary>
    /// Sets the TTL value.
    /// </summary>
    private CreateDomainCommandBuilder WithTtl(int ttl)
    {
        _ttl = ttl;
        return this;
    }

    /// <summary>
    /// Sets the hosting organization.
    /// </summary>
    private CreateDomainCommandBuilder WithHostedAt(string hostedAt)
    {
        _hostedAt = hostedAt;
        return this;
    }

    /// <summary>
    /// Clears the domain field (invalid on purpose).
    /// </summary>
    public CreateDomainCommandBuilder WithoutDomain() => WithDomain(string.Empty);

    /// <summary>
    /// Clears the IP field (invalid on purpose).
    /// </summary>
    public CreateDomainCommandBuilder WithoutIp() => WithIp(string.Empty);

    /// <summary>
    /// Clears the WHOIS field (invalid on purpose).
    /// </summary>
    public CreateDomainCommandBuilder WithoutWhoIs() => WithWhoIs(string.Empty);

    /// <summary>
    /// Clears the host field (invalid on purpose).
    /// </summary>
    public CreateDomainCommandBuilder WithoutHost() => WithHostedAt(string.Empty);

    /// <summary>
    /// Forces a negative TTL (invalid on purpose).
    /// </summary>
    public CreateDomainCommandBuilder WithNegativeTtl() => WithTtl(-1);
}