using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Umbler.WhoIs.Application.UseCases.Domain.Create;
using Umbler.WhoIs.Domain.Repositories;
using Umbler.WhoIs.UnitTests.Application.UseCases.Domain.Builders;

namespace Umbler.WhoIs.UnitTests.Application.UseCases.Domain;

/// <summary>
/// Handler test using the real AutoMapper profile (integration-style mapping check).
/// </summary>
public class CreateDomainCommandHandlerWithMapperTests
{
    private readonly IDomainRepository _repo = Substitute.For<IDomainRepository>();
    private readonly IMapper _mapper;
    private readonly CreateDomainCommandHandler _sut;

    /// <summary>
    /// Registers AutoMapper with the real profile.
    /// </summary>
    public CreateDomainCommandHandlerWithMapperTests()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile(new CreateDomainCommandProfile()));

        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
        _sut = new CreateDomainCommandHandler(_repo, _mapper);
    }

    [Fact(DisplayName = "Handler maps with real profile and persists")]
    public async Task Handle_With_Real_MapperProfile_Works()
    {
        // Arrange
        var cmd = new CreateDomainCommandBuilder().Build();

        // Repository returns the entity it received (simulate persistence echo)
        _repo.CreateDomainAsync(Arg.Any<Umbler.WhoIs.Domain.Entities.Domain>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Umbler.WhoIs.Domain.Entities.Domain>());

        // Act
        var result = await _sut.Handle(cmd, CancellationToken.None);

        // Assert
        result.DomainName.Should().Be(cmd.DomainName);
        result.IpAddress.Should().Be(cmd.IpAddress);
        result.HostedAt.Should().Be(cmd.HostedAt);

        await _repo.Received(1)
            .CreateDomainAsync(Arg.Any<Umbler.WhoIs.Domain.Entities.Domain>(), Arg.Any<CancellationToken>());
    }
}