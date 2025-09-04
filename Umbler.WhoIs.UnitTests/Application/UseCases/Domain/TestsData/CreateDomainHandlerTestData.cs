using Bogus;
using Umbler.WhoIs.Application.UseCases.Domain.Create;

namespace Umbler.WhoIs.UnitTests.Application.UseCases.Domain.TestsData;

/// <summary>
/// Test data factory for <see cref="CreateDomainCommand"/> using Bogus.
/// </summary>
public static class CreateDomainHandlerTestData
{
    /// <summary>
    /// Faker with sensible defaults for a valid command.
    /// </summary>
    private static readonly Faker<CreateDomainCommand> CreateDomainHandlerFaker =
        new Faker<CreateDomainCommand>()
            .RuleFor(c => c.DomainName, f => f.Internet.DomainName())
            .RuleFor(c => c.IpAddress, f => f.Internet.Ip()) // IPv4
            .RuleFor(c => c.WhoIs, _ => "json")
            .RuleFor(c => c.Ttl, f => f.Random.Int(1, 86400))
            .RuleFor(c => c.HostedAt, _ => "Umbler");

    /// <summary>
    /// Generates a valid command instance.
    /// </summary>
    public static CreateDomainCommand GenerateValidCommand() => CreateDomainHandlerFaker.Generate();
}