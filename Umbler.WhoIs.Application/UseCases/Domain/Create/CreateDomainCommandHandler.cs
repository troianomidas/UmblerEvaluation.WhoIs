using AutoMapper;
using FluentValidation;
using MediatR;
using Umbler.WhoIs.Domain.Common;
using Umbler.WhoIs.Domain.Repositories;

namespace Umbler.WhoIs.Application.UseCases.Domain.Create;

/// <summary>
/// Handles CreateDomainCommand: validates input, checks cache by TTL, persists if needed, and maps the response.
/// </summary>
/// <param name="repository">Domain repository.</param>
/// <param name="mapper">AutoMapper instance.</param>
public class CreateDomainCommandHandler(IDomainRepository repository, IMapper mapper)
    : IRequestHandler<CreateDomainCommand, CreateDomainCommandResponse>
{
    /// <summary>
    /// Processes the command, returning an existing fresh record or creating a new one.
    /// </summary>
    /// <param name="command">Create domain request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>DTO with key domain fields.</returns>
    /// <exception cref="ValidationException">Thrown when command data is invalid.</exception>
    public async Task<CreateDomainCommandResponse> Handle(CreateDomainCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateDomainCommandValidator();
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // if exists is “fresh”, return existent
        var existing = await repository.GetByDomainNameAsync(command.DomainName!, cancellationToken);
        if (existing is not null && existing.IsFresh(DateTime.UtcNow))
            return mapper.Map<CreateDomainCommandResponse>(existing);

        // create new record
        var entity = mapper.Map<WhoIs.Domain.Entities.Domain>(command);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = null;

        var saved = await repository.CreateDomainAsync(entity, cancellationToken);
        return mapper.Map<CreateDomainCommandResponse>(saved);
    }
}