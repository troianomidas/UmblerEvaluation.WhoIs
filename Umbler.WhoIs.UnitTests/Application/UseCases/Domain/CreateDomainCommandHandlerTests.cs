using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Umbler.WhoIs.Application.UseCases.Domain.Create;
using Umbler.WhoIs.Domain.Repositories;
using Umbler.WhoIs.UnitTests.Application.UseCases.Domain.Builders;

namespace Umbler.WhoIs.UnitTests.Application.UseCases.Domain;

/// <summary>
/// Pure unit tests for the handler using mocked repository and mapper.
/// </summary>
public class CreateDomainCommandHandlerTests
{
    private readonly IDomainRepository _repo = Substitute.For<IDomainRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CreateDomainCommandHandler _sut;

    public CreateDomainCommandHandlerTests()
    {
        _sut = new CreateDomainCommandHandler(_repo, _mapper);
    }

    [Fact(DisplayName = "Given valid command When handling Then persists and returns response")]
    public async Task Handle_Valid_Should_Map_Persist_And_Return_Response()
    {
        // Arrange
        var cmd = new CreateDomainCommandBuilder().Build();

        var entity = new Umbler.WhoIs.Domain.Entities.Domain
        {
            DomainName = cmd.DomainName,
            IpAddress = cmd.IpAddress,
            WhoIs = cmd.WhoIs,
            Ttl = cmd.Ttl,
            HostedAt = cmd.HostedAt
        };

        var response = new CreateDomainCommandResponse(cmd.DomainName, cmd.IpAddress, cmd.HostedAt);

        _mapper.Map<Umbler.WhoIs.Domain.Entities.Domain>(cmd).Returns(entity);
        _repo.CreateDomainAsync(entity, Arg.Any<CancellationToken>()).Returns(entity);
        _mapper.Map<CreateDomainCommandResponse>(entity).Returns(response);

        // Act
        var result = await _sut.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(response);
        await _repo.Received(1).CreateDomainAsync(entity, Arg.Any<CancellationToken>());
    }
}