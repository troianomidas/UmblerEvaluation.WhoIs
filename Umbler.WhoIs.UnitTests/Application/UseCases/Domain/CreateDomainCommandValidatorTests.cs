using FluentAssertions;
using Umbler.WhoIs.Application.UseCases.Domain.Create;
using Umbler.WhoIs.UnitTests.Application.UseCases.Domain.Builders;

namespace Umbler.WhoIs.UnitTests.Application.UseCases.Domain;

/// <summary>
/// Validator unit tests for <see cref="CreateDomainCommandValidator"/>.
/// </summary>
public class CreateDomainCommandValidatorTests
{
    private readonly CreateDomainCommandValidator _validator = new();

    [Fact(DisplayName = "Given valid command When validating Then is valid")]
    public void Valid_Command_Should_BeValid()
    {
        // Arrange
        var cmd = new CreateDomainCommandBuilder().Build();

        // Act
        var result = _validator.Validate(cmd);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "Given invalid command When validating Then is invalid")]
    [InlineData("", "177.55.66.99", "json", 30, "Umbler")] // empty domain
    [InlineData("invalid..", "177.55.66.99", "json", 30, "Umbler")] // invalid domain
    [InlineData("ns254.umbler.com", "", "json", 30, "Umbler")] // empty IP
    [InlineData("ns254.umbler.com", "177.55.66.99", "", 30, "Umbler")] // empty whois
    [InlineData("ns254.umbler.com", "177.55.66.99", "json", -1, "Umbler")] // negative TTL
    [InlineData("ns254.umbler.com", "177.55.66.99", "json", 30, "")] // empty hostedAt
    public void Invalid_Command_Should_Fail(string domainName, string ipAddress, string whoIs, int ttl, string hostedAt)
    {
        // Arrange
        var cmd = new CreateDomainCommand(domainName, ipAddress, whoIs, ttl, hostedAt);

        // Act
        var result = _validator.Validate(cmd);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}