using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Umbler.WhoIs.Application.UseCases.Domain.Create;
using Umbler.WhoIs.Common.Validations;
using Umbler.WhoIs.Domain.Repositories;
using Umbler.WhoIs.UnitTests.Application.UseCases.Domain.Builders;

namespace Umbler.WhoIs.UnitTests.Crosscutting;

/// <summary>
/// End-to-end pipeline tests (MediatR + ValidationBehavior + AutoMapper + repo mock).
/// </summary>
public class MediatorPipelineTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDomainRepository _repository = Substitute.For<IDomainRepository>();

    public MediatorPipelineTests()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        // MediatR + Handler
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateDomainCommandHandler>());

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateDomainCommandValidator>();

        // Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // AutoMapper profile
        services.AddAutoMapper(cfg => cfg.AddProfile(new CreateDomainCommandProfile()));

        // Repo mock
        services.AddSingleton(_repository);

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact(DisplayName = "Pipeline: invalid command throws ValidationException before handler")]
    public async Task Invalid_Command_Should_Fail_Before_Handler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var invalid = new CreateDomainCommandBuilder().WithoutIp().Build();

        // Act
        var act = async () => await mediator.Send(invalid);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        await _repository.DidNotReceiveWithAnyArgs().CreateDomainAsync(default!, default);
    }

    [Fact(DisplayName = "Pipeline: valid command passes validation and calls repository")]
    public async Task Valid_Command_Should_Persist()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();

        _repository.CreateDomainAsync(Arg.Any<Umbler.WhoIs.Domain.Entities.Domain>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Umbler.WhoIs.Domain.Entities.Domain>());

        var cmd = new CreateDomainCommandBuilder().Build();

        // Act
        var result = await mediator.Send(cmd);

        // Assert
        result.DomainName.Should().Be(cmd.DomainName);
        await _repository.Received(1)
            .CreateDomainAsync(Arg.Any<Umbler.WhoIs.Domain.Entities.Domain>(), Arg.Any<CancellationToken>());
    }
}