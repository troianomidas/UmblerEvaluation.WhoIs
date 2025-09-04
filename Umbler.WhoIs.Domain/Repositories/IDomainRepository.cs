namespace Umbler.WhoIs.Domain.Repositories;

public interface IDomainRepository
{
    /// <summary>
    /// Persists a new domain entity.
    /// </summary>
    /// <param name="domain">Domain entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Entities.Domain> CreateDomainAsync(Entities.Domain domain, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a domain by its DomainName.
    /// </summary>
    /// <param name="domainName">Domain name to search.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Domain.Entities.Domain?> GetByDomainNameAsync(string domainName, CancellationToken cancellationToken);
}