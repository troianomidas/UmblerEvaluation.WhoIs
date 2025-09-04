namespace Umbler.WhoIs.UnitTests.Domain.Entities.Domain.Builders;

/// <summary>
/// Fluent builder for the Domain entity used in unit tests.
/// </summary>
public sealed class DomainBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _domainName = "ns254.umbler.com";
    private string _ipAddress = "177.55.66.99";
    private string _whoIs = "json";
    private int _ttl = 30; // rule >= 0
    private string _hostedAt = "Umbler";

    /// <summary>
    /// Builds an entity instance with the current state.
    /// </summary>
    public Umbler.WhoIs.Domain.Entities.Domain Build() =>
        new()
        {
            Id = _id,
            DomainName = _domainName,
            IpAddress = _ipAddress,
            WhoIs = _whoIs,
            Ttl = _ttl,
            HostedAt = _hostedAt
        };

    /// <summary>Sets the Id.</summary>
    public DomainBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    /// <summary>Sets the domain name.</summary>
    public DomainBuilder WithDomain(string domainName)
    {
        _domainName = domainName;
        return this;
    }

    /// <summary>Sets the IP.</summary>
    public DomainBuilder WithIp(string iAddress)
    {
        _ipAddress = iAddress;
        return this;
    }

    /// <summary>Sets the WHOIS.</summary>
    public DomainBuilder WithWhoIs(string whoIs)
    {
        _whoIs = whoIs;
        return this;
    }

    /// <summary>Sets the TTL.</summary>
    public DomainBuilder WithTtl(int ttl)
    {
        _ttl = ttl;
        return this;
    }

    /// <summary>Sets the hosting org.</summary>
    public DomainBuilder WithHostedAt(string hostedAt)
    {
        _hostedAt = hostedAt;
        return this;
    }
}