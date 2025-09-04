namespace Umbler.WhoIs.Domain.Common;

public static class DomainTtlExtensions
{
    /// <summary>
    /// Checks whether the domain data is still fresh based on TTL and timestamps.
    /// </summary>
    /// <param name="d">Domain entity.</param>
    /// <param name="nowUtc">Current UTC time.</param>
    /// <returns>True if fresh; otherwise false.</returns>
    public static bool IsFresh(this WhoIs.Domain.Entities.Domain d, DateTime nowUtc)
    {
        if (d.Ttl <= 0) return false;
        var basis = d.UpdatedAt ?? d.CreatedAt;
        return basis.AddSeconds(d.Ttl) > nowUtc;
    }
}