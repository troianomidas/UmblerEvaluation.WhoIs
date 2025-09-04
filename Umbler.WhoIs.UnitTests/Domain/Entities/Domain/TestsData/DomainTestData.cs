using Bogus;

namespace Umbler.WhoIs.UnitTests.Domain.Entities.Domain.TestsData;

/// <summary>
/// Bogus factory for creating valid Domain entities for tests.
/// </summary>
public static class DomainTestData
{
    /// <summary>
    /// Faker that generates a consistent, valid Domain entity.
    /// </summary>
    private static readonly Faker<Umbler.WhoIs.Domain.Entities.Domain> DomainFaker =
        new Faker<Umbler.WhoIs.Domain.Entities.Domain>().CustomInstantiator(faker =>
            new Umbler.WhoIs.Domain.Entities.Domain
            {
                Id = Guid.NewGuid(),
                IpAddress = "177.55.66.99",
                WhoIs = "json",
                DomainName = "ns254.umbler.com",
                HostedAt = "Umbler",
                Ttl = 30
            });

    /// <summary>
    /// Generates one valid Domain entity.
    /// </summary>
    public static WhoIs.Domain.Entities.Domain GenerateValidCommand() => DomainFaker.Generate();
}