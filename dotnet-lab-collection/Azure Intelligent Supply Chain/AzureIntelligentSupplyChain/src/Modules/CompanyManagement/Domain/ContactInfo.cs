namespace AzureIntelligentSupplyChain.Modules.CompanyManagement.Domain;

/// <summary>
/// Contact information value object.
/// </summary>
public class ContactInfo : ValueObject
{
    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Phone number.
    /// </summary>
    public string Phone { get; private set; } = string.Empty;

    /// <summary>
    /// Website URL.
    /// </summary>
    public string? Website { get; private set; }

    public ContactInfo()
    {
    }

    public ContactInfo(string email, string phone, string? website = null)
    {
        Email = email;
        Phone = phone;
        Website = website;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
        yield return Phone;
        yield return Website;
    }

    public override string ToString()
    {
        return $"Email: {Email}, Phone: {Phone}";
    }
}
