using Microsoft.EntityFrameworkCore;
using Umbler.WhoIs.Domain.Repositories;

namespace Umbler.WhoIs.ORM.Repositories;

/// <summary>
/// Repository implementation for Domain using EF Core.
/// </summary>
/// <param name="context">EF Core DbContext.</param>
public class DomainRepository(DefaultContext context) : IDomainRepository
{
    /// <summary>
    /// Persists a new Domain entity to the database.
    /// </summary>
    /// <param name="domain">Domain to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The persisted Domain entity.</returns>
    public async Task<Domain.Entities.Domain> CreateDomainAsync(Domain.Entities.Domain domain,
        CancellationToken cancellationToken)
    {
        await context.Domains.AddAsync(domain, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return domain;
    }

    /// <summary>
    /// Retrieves a Domain by its DomainName, or null if not found.
    /// </summary>
    /// <param name="domainName">Domain name to search.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Domain found or null.</returns>
    public async Task<Domain.Entities.Domain?> GetByDomainNameAsync(string domainName,
        CancellationToken cancellationToken) =>
        await context.Domains.AsNoTracking()
            .FirstOrDefaultAsync(domain => domain.DomainName == domainName, cancellationToken);
}