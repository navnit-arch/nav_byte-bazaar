namespace AzureIntelligentSupplyChain.Modules.CompanyManagement.Domain;

/// <summary>
/// Address value object.
/// </summary>
public class Address : ValueObject
{
    /// <summary>
    /// Street address.
    /// </summary>
    public string Street { get; private set; } = string.Empty;

    /// <summary>
    /// City.
    /// </summary>
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// State/Province.
    /// </summary>
    public string State { get; private set; } = string.Empty;

    /// <summary>
    /// Postal code.
    /// </summary>
    public string PostalCode { get; private set; } = string.Empty;

    /// <summary>
    /// Country.
    /// </summary>
    public string Country { get; private set; } = string.Empty;

    public Address()
    {
    }

    public Address(string street, string city, string state, string postalCode, string country)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return PostalCode;
        yield return Country;
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State} {PostalCode}, {Country}";
    }
}
