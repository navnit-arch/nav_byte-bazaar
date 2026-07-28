namespace AzureIntelligentSupplyChain.Modules.CompanyManagement.Domain;

/// <summary>
/// Company aggregate root representing a company in the system.
/// </summary>
[BsonCollection("companies")]
public class Company : AggregateRoot
{
    /// <summary>
    /// Company name.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Company registration number.
    /// </summary>
    [BsonElement("registrationNumber")]
    public string RegistrationNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Tax identification number.
    /// </summary>
    [BsonElement("taxNumber")]
    public string TaxNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Company description.
    /// </summary>
    [BsonElement("description")]
    public string? Description { get; private set; }

    /// <summary>
    /// Headquarters address.
    /// </summary>
    [BsonElement("headquarters")]
    public Address? Headquarters { get; private set; }

    /// <summary>
    /// Contact information.
    /// </summary>
    [BsonElement("contact")]
    public ContactInfo? Contact { get; private set; }

    /// <summary>
    /// Company status.
    /// </summary>
    [BsonElement("status")]
    public CompanyStatus Status { get; private set; } = CompanyStatus.Active;

    /// <summary>
    /// Company type.
    /// </summary>
    [BsonElement("companyType")]
    public CompanyType CompanyType { get; private set; }

    /// <summary>
    /// Industry classification.
    /// </summary>
    [BsonElement("industry")]
    public string? Industry { get; private set; }

    /// <summary>
    /// Number of employees.
    /// </summary>
    [BsonElement("numberOfEmployees")]
    public int? NumberOfEmployees { get; private set; }

    /// <summary>
    /// Annual revenue.
    /// </summary>
    [BsonElement("annualRevenue")]
    public decimal? AnnualRevenue { get; private set; }

    /// <summary>
    /// Company logo URL.
    /// </summary>
    [BsonElement("logoUrl")]
    public string? LogoUrl { get; private set; }

    /// <summary>
    /// Bank account information.
    /// </summary>
    [BsonElement("bankAccounts")]
    public List<BankAccount> BankAccounts { get; private set; } = new();

    public Company()
    {
    }

    /// <summary>
    /// Creates a new company.
    /// </summary>
    public Company(
        string name,
        string registrationNumber,
        string taxNumber,
        CompanyType companyType,
        Address? headquarters = null,
        ContactInfo? contact = null,
        string? description = null,
        string? industry = null,
        int? numberOfEmployees = null,
        decimal? annualRevenue = null)
        : base()
    {
        Name = name;
        RegistrationNumber = registrationNumber;
        TaxNumber = taxNumber;
        CompanyType = companyType;
        Headquarters = headquarters;
        Contact = contact;
        Description = description;
        Industry = industry;
        NumberOfEmployees = numberOfEmployees;
        AnnualRevenue = annualRevenue;
        Status = CompanyStatus.Active;
    }

    /// <summary>
    /// Updates company information.
    /// </summary>
    public void Update(
        string name,
        string? description = null,
        Address? headquarters = null,
        ContactInfo? contact = null,
        string? industry = null,
        int? numberOfEmployees = null,
        decimal? annualRevenue = null,
        string? logoUrl = null)
    {
        Name = name;
        if (description != null) Description = description;
        if (headquarters != null) Headquarters = headquarters;
        if (contact != null) Contact = contact;
        if (industry != null) Industry = industry;
        if (numberOfEmployees.HasValue) NumberOfEmployees = numberOfEmployees;
        if (annualRevenue.HasValue) AnnualRevenue = annualRevenue;
        if (logoUrl != null) LogoUrl = logoUrl;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the company.
    /// </summary>
    public void Activate()
    {
        if (Status == CompanyStatus.Active)
            throw new BusinessRuleException("Company is already active.");

        Status = CompanyStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the company.
    /// </summary>
    public void Deactivate()
    {
        if (Status == CompanyStatus.Inactive)
            throw new BusinessRuleException("Company is already inactive.");

        Status = CompanyStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a bank account.
    /// </summary>
    public void AddBankAccount(BankAccount bankAccount)
    {
        if (BankAccounts.Any(b => b.AccountNumber == bankAccount.AccountNumber))
            throw new BusinessRuleException("Bank account already exists.");

        BankAccounts.Add(bankAccount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a bank account.
    /// </summary>
    public void RemoveBankAccount(string accountNumber)
    {
        var bankAccount = BankAccounts.FirstOrDefault(b => b.AccountNumber == accountNumber);
        if (bankAccount == null)
            throw new NotFoundException("Bank account", accountNumber);

        BankAccounts.Remove(bankAccount);
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Company status enumeration.
/// </summary>
public enum CompanyStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3,
    Deleted = 4
}

/// <summary>
/// Company type enumeration.
/// </summary>
public enum CompanyType
{
    Internal = 1,
    Supplier = 2,
    Customer = 3,
    Logistics = 4,
    Manufacturer = 5
}

/// <summary>
/// Bank account value object.
/// </summary>
public class BankAccount : ValueObject
{
    /// <summary>
    /// Account holder name.
    /// </summary>
    public string AccountHolderName { get; private set; } = string.Empty;

    /// <summary>
    /// Account number.
    /// </summary>
    public string AccountNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Bank name.
    /// </summary>
    public string BankName { get; private set; } = string.Empty;

    /// <summary>
    /// IBAN.
    /// </summary>
    public string? IBAN { get; private set; }

    /// <summary>
    /// SWIFT code.
    /// </summary>
    public string? SWIFTCode { get; private set; }

    /// <summary>
    /// Account type.
    /// </summary>
    public string? AccountType { get; private set; }

    /// <summary>
    /// Is default account.
    /// </summary>
    public bool IsDefault { get; private set; }

    public BankAccount()
    {
    }

    public BankAccount(
        string accountHolderName,
        string accountNumber,
        string bankName,
        string? iban = null,
        string? swiftCode = null,
        string? accountType = null,
        bool isDefault = false)
    {
        AccountHolderName = accountHolderName;
        AccountNumber = accountNumber;
        BankName = bankName;
        IBAN = iban;
        SWIFTCode = swiftCode;
        AccountType = accountType;
        IsDefault = isDefault;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AccountNumber;
        yield return BankName;
        yield return IBAN;
        yield return SWIFTCode;
    }

    public override string ToString()
    {
        return $"{BankName} - {AccountNumber}";
    }
}
