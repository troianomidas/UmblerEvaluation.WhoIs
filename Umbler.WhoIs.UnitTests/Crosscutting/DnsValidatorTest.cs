using Umbler.WhoIs.Common.Validations.Utils;

namespace Umbler.WhoIs.UnitTests.Crosscutting;

/// <summary>
/// Tests for <see cref="DnsValidator"/> helpers.
/// </summary>
public class DnsValidatorTest
{
    [Theory(DisplayName = "IsValidDomainName returns true for valid domains")]
    [InlineData("teste.com")]
    [InlineData("teste.com.br")]
    [InlineData("teste.org")]
    [InlineData("teste.teste.org")]
    [InlineData("123.teste.org")]
    [InlineData("teste.123.org")]
    [InlineData("teste-teste.com")]
    public void Should_ReturnTrue_If_GivenDnsIsValid(string domainName)
    {
        // Act & Assert
        Assert.True(DnsValidator.IsValidDomainName(domainName));
    }

    [Theory(DisplayName = "IsValidDomainName returns false for invalid domains")]
    [InlineData("")]
    [InlineData("teste")]
    [InlineData(".org")]
    [InlineData("127.0.0.0")]
    [InlineData("-teste.com")]
    [InlineData("?teste.com")]
    [InlineData(".teste.com")]
    [InlineData("#teste.com")]
    [InlineData("foo.-bar.com")]
    public void Should_ReturnFalse_If_GivenDnsIsInvalid(string domainName)
    {
        // Act & Assert
        Assert.False(DnsValidator.IsValidDomainName(domainName));
    }

    [Theory(DisplayName = "IsValidIPv4Address returns true for valid IPv4")]
    [InlineData("172.25.80.1")]
    [InlineData("0.0.0.0")]
    [InlineData("127.0.0.0")]
    [InlineData("255.255.255.255")]
    public void Should_ReturnTrue_If_GivenIpv4IsValid(string ipv4Address)
    {
        // Act & Assert
        Assert.True(DnsValidator.IsValidIPv4Address(ipv4Address));
    }

    [Theory(DisplayName = "IsValidIPv4Address returns false for invalid IPv4")]
    [InlineData("")]
    [InlineData("256.1.1.1")]
    [InlineData("1.1.1")]
    [InlineData("1.1.1.1.1")]
    [InlineData("01.2.3.4")]
    [InlineData("1..1.1")]
    [InlineData("1.1.1.-1")]
    public void Should_ReturnFalse_If_GivenIpv4IsInvalid(string ipv4Address)
    {
        // Act & Assert
        Assert.False(DnsValidator.IsValidIPv4Address(ipv4Address));
    }
}