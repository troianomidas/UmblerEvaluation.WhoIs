using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Umbler.WhoIs.Common.Validations.Utils;

/// <summary>
/// DNS-related validation helpers (domain and IPv4 address).
/// </summary>
public static partial class DnsValidator
{
    /// <summary>
    /// Validates a fully-qualified domain name using a simplified regex.
    /// </summary>
    /// <param name="domainName">Domain to validate.</param>
    /// <returns>True when the domain name matches the pattern.</returns>
    public static bool IsValidDomainName(string domainName)
    {
        return DomainNameRegex().IsMatch(domainName);
    }

    /// <summary>
    /// Validates an IPv4 address (basic checks + IPAddress.TryParse).
    /// </summary>
    /// <param name="ipAddress">IPv4 string.</param>
    /// <returns>True if a valid IPv4 address.</returns>
    public static bool IsValidIPv4Address(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress)) return false;
        if (ipAddress.Contains(' ') || ipAddress.Count(c => c == '.') != 3) return false;
        if (ipAddress.Split('.').Any(p => p.Length > 1 && p.StartsWith("0"))) return false;

        // TryParse with IPAddress is better than maintaining another custom regex here
        return IPAddress.TryParse(ipAddress, out var addr) &&
               addr.AddressFamily == AddressFamily.InterNetwork;
    }

    /// <summary>
    /// Basic domain name regex (does not fully cover IDNs or all edge cases).
    /// </summary>
    [GeneratedRegex(@"^(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.(?!-)[A-Za-z0-9-]{1,63}(?<!-))*(\.[A-Za-z]{2,63})$")]
    private static partial Regex DomainNameRegex();
}