namespace AzureIntelligentSupplyChain.Modules.CompanyManagement.Application.Validations;

/// <summary>
/// Validator for CreateCompanyDto.
/// </summary>
public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(255).WithMessage("Company name must not exceed 255 characters");

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty().WithMessage("Registration number is required")
            .MaximumLength(50).WithMessage("Registration number must not exceed 50 characters");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Tax number is required")
            .MaximumLength(50).WithMessage("Tax number must not exceed 50 characters");

        RuleFor(x => x.CompanyType)
            .GreaterThanOrEqualTo(1).WithMessage("Company type must be between 1 and 5")
            .LessThanOrEqualTo(5).WithMessage("Company type must be between 1 and 5");

        RuleFor(x => x.NumberOfEmployees)
            .GreaterThan(0).When(x => x.NumberOfEmployees.HasValue)
            .WithMessage("Number of employees must be greater than 0");

        RuleFor(x => x.AnnualRevenue)
            .GreaterThan(0).When(x => x.AnnualRevenue.HasValue)
            .WithMessage("Annual revenue must be greater than 0");

        RuleFor(x => x.Contact)
            .SetValidator(new ContactInfoDtoValidator()!)
            .When(x => x.Contact != null);

        RuleFor(x => x.Headquarters)
            .SetValidator(new AddressDtoValidator()!)
            .When(x => x.Headquarters != null);
    }
}

/// <summary>
/// Validator for UpdateCompanyDto.
/// </summary>
public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
{
    public UpdateCompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(255).WithMessage("Company name must not exceed 255 characters");

        RuleFor(x => x.NumberOfEmployees)
            .GreaterThan(0).When(x => x.NumberOfEmployees.HasValue)
            .WithMessage("Number of employees must be greater than 0");

        RuleFor(x => x.AnnualRevenue)
            .GreaterThan(0).When(x => x.AnnualRevenue.HasValue)
            .WithMessage("Annual revenue must be greater than 0");

        RuleFor(x => x.Contact)
            .SetValidator(new ContactInfoDtoValidator()!)
            .When(x => x.Contact != null);

        RuleFor(x => x.Headquarters)
            .SetValidator(new AddressDtoValidator()!)
            .When(x => x.Headquarters != null);
    }
}

/// <summary>
/// Validator for AddressDto.
/// </summary>
public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(255).WithMessage("Street must not exceed 255 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State/Province is required")
            .MaximumLength(100).WithMessage("State/Province must not exceed 100 characters");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required")
            .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters");
    }
}

/// <summary>
/// Validator for ContactInfoDto.
/// </summary>
public class ContactInfoDtoValidator : AbstractValidator<ContactInfoDto>
{
    public ContactInfoDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone must be a valid phone number");

        RuleFor(x => x.Website)
            .Must(x => x == null || Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Website must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.Website));
    }
}

/// <summary>
/// Validator for BankAccountDto.
/// </summary>
public class BankAccountDtoValidator : AbstractValidator<BankAccountDto>
{
    public BankAccountDtoValidator()
    {
        RuleFor(x => x.AccountHolderName)
            .NotEmpty().WithMessage("Account holder name is required")
            .MaximumLength(255).WithMessage("Account holder name must not exceed 255 characters");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .MaximumLength(50).WithMessage("Account number must not exceed 50 characters");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("Bank name is required")
            .MaximumLength(255).WithMessage("Bank name must not exceed 255 characters");

        RuleFor(x => x.IBAN)
            .Must(x => x == null || IsValidIBAN(x))
            .WithMessage("IBAN must be a valid IBAN")
            .When(x => !string.IsNullOrEmpty(x.IBAN));

        RuleFor(x => x.SWIFTCode)
            .Must(x => x == null || IsValidSWIFT(x))
            .WithMessage("SWIFT code must be valid (8 or 11 characters)")
            .When(x => !string.IsNullOrEmpty(x.SWIFTCode));
    }

    private static bool IsValidIBAN(string iban)
    {
        return !string.IsNullOrEmpty(iban) && iban.Length >= 15 && iban.Length <= 34;
    }

    private static bool IsValidSWIFT(string swift)
    {
        return !string.IsNullOrEmpty(swift) && (swift.Length == 8 || swift.Length == 11);
    }
}
