using FluentAssertions;
using Umbler.WhoIs.UnitTests.Domain.Entities.Domain.Builders;

namespace Umbler.WhoIs.UnitTests.Domain.Entities.Domain;

/// <summary>
/// Validation tests for the Domain entity.
/// </summary>
public class DomainTests
{
    [Fact(DisplayName = "Given valid Domain entity When validating Then returns valid")]
    public void Given_ValidDomain_Should_ReturnValid()
    {
        // Arrange
        var domain = new DomainBuilder().Build();

        // Act
        var result = domain.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory(DisplayName = "Given missing fields When validating Then returns invalid")]
    [InlineData("", "json", "ns254.umbler.com", "Umbler")]           // empty IP
    [InlineData("177.55.66.99", "", "ns254.umbler.com", "Umbler")]   // empty WHOIS
    [InlineData("177.55.66.99", "json", "", "Umbler")]               // empty DomainName
    [InlineData("177.55.66.99", "json", "ns254.umbler.com", "")]     // empty HostedAt
    public void Given_InvalidFields_Should_ReturnInvalid(string ip, string whois, string domainName, string hostedAt)
    {
        // Arrange
        var domain = new DomainBuilder()
            .WithIp(ip)
            .WithWhoIs(whois)
            .WithDomain(domainName)
            .WithHostedAt(hostedAt)
            .Build();

        // Act
        var result = domain.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}