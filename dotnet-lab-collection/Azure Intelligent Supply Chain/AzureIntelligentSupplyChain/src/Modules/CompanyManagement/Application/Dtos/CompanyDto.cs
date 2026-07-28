namespace AzureIntelligentSupplyChain.Modules.CompanyManagement.Application.Dtos;

/// <summary>
/// DTO for address information.
/// </summary>
public class AddressDto
{
    /// <summary>
    /// Street address.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// City.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State/Province.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Postal code.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Country.
    /// </summary>
    public string Country { get; set; } = string.Empty;
}

/// <summary>
/// DTO for contact information.
/// </summary>
public class ContactInfoDto
{
    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Website URL.
    /// </summary>
    public string? Website { get; set; }
}

/// <summary>
/// DTO for bank account information.
/// </summary>
public class BankAccountDto
{
    /// <summary>
    /// Account holder name.
    /// </summary>
    public string AccountHolderName { get; set; } = string.Empty;

    /// <summary>
    /// Account number.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Bank name.
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// IBAN.
    /// </summary>
    public string? IBAN { get; set; }

    /// <summary>
    /// SWIFT code.
    /// </summary>
    public string? SWIFTCode { get; set; }

    /// <summary>
    /// Account type.
    /// </summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// Is default account.
    /// </summary>
    public bool IsDefault { get; set; }
}

/// <summary>
/// DTO for creating a company.
/// </summary>
public class CreateCompanyDto
{
    /// <summary>
    /// Company name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Company registration number.
    /// </summary>
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Tax identification number.
    /// </summary>
    public string TaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// Company type (1: Internal, 2: Supplier, 3: Customer, 4: Logistics, 5: Manufacturer).
    /// </summary>
    public int CompanyType { get; set; }

    /// <summary>
    /// Company description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Headquarters address.
    /// </summary>
    public AddressDto? Headquarters { get; set; }

    /// <summary>
    /// Contact information.
    /// </summary>
    public ContactInfoDto? Contact { get; set; }

    /// <summary>
    /// Industry classification.
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// Number of employees.
    /// </summary>
    public int? NumberOfEmployees { get; set; }

    /// <summary>
    /// Annual revenue.
    /// </summary>
    public decimal? AnnualRevenue { get; set; }
}

/// <summary>
/// DTO for updating a company.
/// </summary>
public class UpdateCompanyDto
{
    /// <summary>
    /// Company name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Company description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Headquarters address.
    /// </summary>
    public AddressDto? Headquarters { get; set; }

    /// <summary>
    /// Contact information.
    /// </summary>
    public ContactInfoDto? Contact { get; set; }

    /// <summary>
    /// Industry classification.
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// Number of employees.
    /// </summary>
    public int? NumberOfEmployees { get; set; }

    /// <summary>
    /// Annual revenue.
    /// </summary>
    public decimal? AnnualRevenue { get; set; }

    /// <summary>
    /// Company logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }
}

/// <summary>
/// DTO for company response.
/// </summary>
public class CompanyDto : BaseDto
{
    /// <summary>
    /// Company name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Company registration number.
    /// </summary>
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Tax identification number.
    /// </summary>
    public string TaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// Company description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Headquarters address.
    /// </summary>
    public AddressDto? Headquarters { get; set; }

    /// <summary>
    /// Contact information.
    /// </summary>
    public ContactInfoDto? Contact { get; set; }

    /// <summary>
    /// Company status (1: Active, 2: Inactive, 3: Suspended, 4: Deleted).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Company type (1: Internal, 2: Supplier, 3: Customer, 4: Logistics, 5: Manufacturer).
    /// </summary>
    public int CompanyType { get; set; }

    /// <summary>
    /// Industry classification.
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// Number of employees.
    /// </summary>
    public int? NumberOfEmployees { get; set; }

    /// <summary>
    /// Annual revenue.
    /// </summary>
    public decimal? AnnualRevenue { get; set; }

    /// <summary>
    /// Company logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Bank accounts.
    /// </summary>
    public List<BankAccountDto> BankAccounts { get; set; } = new();
}
