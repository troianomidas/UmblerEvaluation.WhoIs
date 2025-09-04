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
/// Minimal happy-path pipeline test to ensure the command reaches the repo.
/// </summary>
public class PipelineValidCommandTests
{
    [Fact(DisplayName = "Pipeline: valid command flows through and hits repository")]
    public async Task Valid_Command_Goes_Through_Pipeline_And_Hits_Repo()
    {
        // Arrange
        var services = new ServiceCollection();
        var repo = Substitute.For<IDomainRepository>();

        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateDomainCommandHandler>());
        services.AddValidatorsFromAssemblyContaining<CreateDomainCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddAutoMapper(cfg => cfg.AddProfile(new CreateDomainCommandProfile()));
        services.AddSingleton(repo);

        var sp = services.BuildServiceProvider();
        var mediator = sp.GetRequiredService<IMediator>();

        repo.CreateDomainAsync(Arg.Any<Umbler.WhoIs.Domain.Entities.Domain>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Umbler.WhoIs.Domain.Entities.Domain>());

        var cmd = new CreateDomainCommandBuilder().Build();

        // Act
        var result = await mediator.Send(cmd);

        // Assert
        result.DomainName.Should().Be(cmd.DomainName);
        await repo.Received(1)
            .CreateDomainAsync(Arg.Any<Umbler.WhoIs.Domain.Entities.Domain>(), Arg.Any<CancellationToken>());
    }
}