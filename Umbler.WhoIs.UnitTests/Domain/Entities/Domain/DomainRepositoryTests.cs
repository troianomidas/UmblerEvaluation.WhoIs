using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Umbler.WhoIs.ORM;
using Umbler.WhoIs.ORM.Repositories;

namespace Umbler.WhoIs.UnitTests.Domain.Entities.Domain;

/// <summary>
/// Persistence unit tests for the <see cref="DomainRepository"/> with EF InMemory.
/// </summary>
public class DomainRepositoryTests
{
    [Fact(DisplayName = "Repository persists a Domain entity with EF InMemory")]
    public async Task CreateDomainAsync_Persists_Entity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var ctx = new DefaultContext(options);
        var repo = new DomainRepository(ctx);

        var entity = new Umbler.WhoIs.Domain.Entities.Domain
        {
            DomainName = "ns254.umbler.com",
            IpAddress = "177.55.66.99",
            WhoIs = "json",
            Ttl = 30,
            HostedAt = "Umbler"
        };

        // Act
        var saved = await repo.CreateDomainAsync(entity, CancellationToken.None);

        // Assert
        saved.Id.Should().NotBeEmpty();
        (await ctx.Domains.CountAsync()).Should().Be(1);
    }
}