namespace AzureIntelligentSupplyChain.Infrastructure.Configuration;

/// <summary>
/// Authentication configuration options.
/// </summary>
public class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    /// <summary>
    /// JWT issuer.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// JWT audience.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// JWT secret key.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration in minutes.
    /// </summary>
    public int TokenExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Refresh token expiration in days.
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
