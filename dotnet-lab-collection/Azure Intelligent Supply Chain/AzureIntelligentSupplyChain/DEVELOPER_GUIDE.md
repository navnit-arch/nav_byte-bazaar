# Developer Guide - Azure Intelligent Supply Chain Platform

## Table of Contents

1. [Development Setup](#development-setup)
2. [Project Structure](#project-structure)
3. [Code Standards](#code-standards)
4. [Adding New Features](#adding-new-features)
5. [Database Design](#database-design)
6. [Testing Guidelines](#testing-guidelines)
7. [Deployment](#deployment)
8. [Troubleshooting](#troubleshooting)

---

## Development Setup

### Prerequisites
- .NET 8 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- Visual Studio 2022 or VS Code with C# extension
- MongoDB (Local or Atlas)
- Git

### Initial Setup

```bash
# Clone repository
git clone <repo-url>
cd AzureIntelligentSupplyChain

# Restore packages
dotnet restore

# Update configuration
# Edit appsettings.Development.json with your MongoDB connection

# Build solution
dotnet build

# Run application
dotnet run
```

### Verify Setup
```bash
# Health check
curl https://localhost:5001/health

# Swagger UI
https://localhost:5001/swagger/index.html
```

---

## Project Structure

### Layer Organization

```
src/
??? Core/
?   ??? Domain/          - Business rules, entities, value objects
?   ?   ??? Common/      - Base classes (Entity, ValueObject, etc.)
?   ?   ??? Interfaces/  - Repository and UnitOfWork contracts
?   ??? Application/     - Use cases, DTOs, exceptions
?       ??? Common/      - Shared app services
?
??? Infrastructure/      - External dependencies, database, config
?   ??? Persistence/     - Data access implementations
?   ??? Configuration/   - Options and settings
?   ??? Extensions/      - Dependency injection setup
?
??? Modules/            - Feature modules (organized by domain)
?   ??? [ModuleName]/
?       ??? Domain/      - Module entities and value objects
?       ??? Application/ - Module DTOs, validators, CQRS
?       ??? Api/         - Module controllers
?
??? Tests/             - Unit and integration tests (future)
?
??? Program.cs         - Application entry point
??? appsettings.json   - Configuration files
```

### Dependency Flow

```
Controllers (API)
    ?
CQRS Handlers (MediatR)
    ?
Domain Services / Repository
    ?
MongoDB Repository
    ?
MongoDB
```

**Rule**: Dependencies flow inward (?). Domain never depends on Infrastructure.

---

## Code Standards

### Naming Conventions

```csharp
// Classes - PascalCase
public class CompanyManager { }

// Methods - PascalCase
public async Task<Company> GetCompanyAsync(string id) { }

// Variables/Parameters - camelCase
private string companyName = "";

// Constants - UPPER_SNAKE_CASE
private const int MAX_COMPANY_NAME_LENGTH = 255;

// Private fields - _camelCase
private readonly IRepository<Company> _repository;

// Async methods - suffix with Async
public async Task<CompanyDto> GetCompanyAsync() { }
```

### XML Documentation

Every public class and method must have XML documentation:

```csharp
/// <summary>
/// Creates a new company in the system.
/// </summary>
/// <param name="request">The company creation request.</param>
/// <param name="cancellationToken">The cancellation token.</param>
/// <returns>A response containing the created company.</returns>
public async Task<ApiResponse<CompanyDto>> CreateCompany(
    CreateCompanyDto request,
    CancellationToken cancellationToken)
{
    // Implementation
}
```

### Code Organization

```csharp
public class MyClass
{
    // 1. Constants
    private const string DEFAULT_VALUE = "";

    // 2. Fields
    private readonly ILogger<MyClass> _logger;

    // 3. Constructors
    public MyClass(ILogger<MyClass> logger)
    {
        _logger = logger;
    }

    // 4. Public methods
    public void PublicMethod() { }

    // 5. Private methods
    private void PrivateMethod() { }
}
```

### Error Handling

```csharp
try
{
    // Operation
    await _repository.AddAsync(entity);
}
catch (MongoException ex)
{
    _logger.LogError(ex, "Database error while adding entity");
    throw;
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    throw;
}
```

---

## Adding New Features

### Step 1: Create Domain Model

```csharp
// Domain/Warehouse.cs
[BsonCollection("warehouses")]
public class Warehouse : AggregateRoot
{
    [BsonElement("name")]
    public string Name { get; private set; }

    [BsonElement("location")]
    public Location Location { get; private set; }

    // Business methods
    public void UpdateLocation(Location newLocation)
    {
        Location = newLocation;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

### Step 2: Create DTOs

```csharp
// Application/Dtos/WarehouseDto.cs
public class CreateWarehouseDto
{
    public string Name { get; set; } = string.Empty;
    public LocationDto Location { get; set; } = null!;
}

public class WarehouseDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public LocationDto Location { get; set; } = null!;
}
```

### Step 3: Create Validators

```csharp
// Application/Validations/WarehouseValidators.cs
public class CreateWarehouseDtoValidator : AbstractValidator<CreateWarehouseDto>
{
    public CreateWarehouseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required")
            .MaximumLength(255).WithMessage("Warehouse name must not exceed 255 characters");

        RuleFor(x => x.Location)
            .NotNull().WithMessage("Location is required")
            .SetValidator(new LocationDtoValidator());
    }
}
```

### Step 4: Create AutoMapper Profile

```csharp
// Application/Mappings/WarehouseMappingProfile.cs
public class WarehouseMappingProfile : Profile
{
    public WarehouseMappingProfile()
    {
        CreateMap<Warehouse, WarehouseDto>();

        CreateMap<CreateWarehouseDto, Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
```

### Step 5: Create CQRS Commands/Queries

```csharp
// Application/Commands/CreateWarehouseCommand.cs
public class CreateWarehouseCommand : IRequest<ApiResponse<WarehouseDto>>
{
    public CreateWarehouseDto Warehouse { get; set; } = null!;
    public string? UserId { get; set; }
}

public class CreateWarehouseCommandHandler : 
    IRequestHandler<CreateWarehouseCommand, ApiResponse<WarehouseDto>>
{
    public async Task<ApiResponse<WarehouseDto>> Handle(
        CreateWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var repository = _repositoryFactory.CreateRepository<Warehouse>();
            var warehouse = _mapper.Map<Warehouse>(request.Warehouse);

            await repository.AddAsync(warehouse, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<WarehouseDto>.SuccessResponse(
                _mapper.Map<WarehouseDto>(warehouse),
                "Warehouse created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating warehouse");
            return ApiResponse<WarehouseDto>.FailureResponse(
                "An error occurred while creating the warehouse");
        }
    }
}
```

### Step 6: Create API Controller

```csharp
// Api/Controllers/WarehousesController.cs
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WarehousesController> _logger;

    public WarehousesController(IMediator mediator, ILogger<WarehousesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> CreateWarehouse(
        [FromBody] CreateWarehouseDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateWarehouseCommand { Warehouse = request };
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetWarehouseById), 
            new { id = result.Data?.Id }, result);
    }
}
```

---

## Database Design

### MongoDB Collection Strategy

Each entity gets its own collection with the entity name (plural convention is optional):

```javascript
db.companies
db.warehouses
db.products
db.inventory
```

### Index Creation

Create indexes for frequently queried fields:

```csharp
// In initialization/seeding
var indexes = new List<CreateIndexModel<Company>>
{
    new CreateIndexModel<Company>(
        Builders<Company>.IndexKeys.Ascending(x => x.Status)),
    new CreateIndexModel<Company>(
        Builders<Company>.IndexKeys.Ascending(x => x.CompanyType),
        new CreateIndexOptions { Name = "idx_company_type" })
};

await _context.CreateIndexesAsync<Company>("companies", indexes);
```

### Document Structure

MongoDB documents follow the entity structure:

```json
{
  "_id": ObjectId,
  "name": "Company Name",
  "registrationNumber": "REG-001",
  "status": 1,
  "companyType": 1,
  "headquarters": {
    "street": "123 Main St",
    "city": "New York",
    "state": "NY",
    "postalCode": "10001",
    "country": "USA"
  },
  "contact": {
    "email": "info@company.com",
    "phone": "+1234567890",
    "website": "https://company.com"
  },
  "bankAccounts": [...],
  "createdAt": ISODate("2024-01-15T10:00:00Z"),
  "createdBy": "user-id",
  "updatedAt": ISODate("2024-01-15T10:30:00Z"),
  "updatedBy": "user-id",
  "isDeleted": false,
  "deletedAt": null,
  "version": 2
}
```

---

## Testing Guidelines

### Unit Testing

Test handlers, validators, and business logic:

```csharp
[Fact]
public async Task CreateCompanyCommand_WithValidData_ShouldSucceed()
{
    // Arrange
    var command = new CreateCompanyCommand
    {
        Company = new CreateCompanyDto
        {
            Name = "Test Company",
            RegistrationNumber = "REG-001",
            TaxNumber = "TAX-001",
            CompanyType = 1
        }
    };

    var handler = new CreateCompanyCommandHandler(
        _repositoryFactory,
        _mapper,
        _logger,
        _unitOfWork);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Success);
    Assert.NotNull(result.Data);
}
```

### Integration Testing

Test API endpoints with real database:

```csharp
[Fact]
public async Task Post_Companies_ShouldCreateCompany()
{
    // Arrange
    var client = _factory.CreateClient();
    var createDto = new CreateCompanyDto { ... };

    // Act
    var response = await client.PostAsJsonAsync(
        "/api/v1/companies",
        createDto);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
}
```

---

## Deployment

### Docker Build

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "AzureIntelligentSupplyChain.dll"]
```

### Azure App Service Deployment

```bash
# Create resource group
az group create --name scm-rg --location eastus

# Create App Service plan
az appservice plan create --name scm-plan --resource-group scm-rg --sku B1 --is-linux

# Create web app
az webapp create --resource-group scm-rg --plan scm-plan --name scm-api --runtime "DOTNETCORE|8.0"

# Publish
dotnet publish -c Release -o publish
cd publish
zip -r ../app.zip *
az webapp deployment source config-zip --resource-group scm-rg --name scm-api --src-path ../app.zip
```

---

## Troubleshooting

### Common Issues

#### MongoDB Connection Error
```
Error: "Unable to connect to the MongoDb server"
```
**Solution**:
1. Verify MongoDB is running: `mongosh`
2. Check connection string in `appsettings.json`
3. Verify firewall allows MongoDB port

#### JWT Token Expired
```
Error: "Token has expired"
```
**Solution**:
1. Request new token
2. Check token expiration in `appsettings.json`
3. Verify system clock is synchronized

#### Validation Error
```
Response: 400 Bad Request with validation errors
```
**Solution**:
1. Review error details in response
2. Check input matches validation rules
3. Review FluentValidation setup

#### Database Index Error
```
Error: "Duplicate key error"
```
**Solution**:
1. Check for duplicate data in collection
2. Review index creation strategy
3. Consider rebuilding indexes

### Debug Logging

Enable debug logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  }
}
```

View logs:
```bash
tail -f logs/app-*.txt
```

---

## Performance Tips

1. **Use pagination** - Always limit result sets
2. **Create indexes** - On frequently queried fields
3. **Use projections** - Retrieve only needed fields
4. **Cache results** - For frequently accessed data
5. **Batch operations** - When processing multiple items
6. **Monitor slow queries** - Use MongoDB profiler

---

## Security Best Practices

1. **Never commit secrets** - Use configuration management
2. **Validate all inputs** - Use FluentValidation
3. **Log security events** - Failed auth, unauthorized access
4. **Use HTTPS** - Always in production
5. **Rotate secrets** - Regularly update keys
6. **Audit changes** - Track who changed what and when

---

## Git Workflow

```bash
# Create feature branch
git checkout -b feature/warehouse-management

# Make changes and commit
git add .
git commit -m "feat: add warehouse management"

# Push and create PR
git push origin feature/warehouse-management

# After review and merge, delete branch
git branch -d feature/warehouse-management
```

### Commit Message Format
```
feat: add new feature
fix: fix a bug
docs: update documentation
refactor: code refactoring
test: add tests
chore: maintenance tasks
```

---

## Release Process

1. **Version bump** - Update version in csproj
2. **Update CHANGELOG** - Document changes
3. **Create tag** - `git tag v1.1.0`
4. **Build release** - `dotnet build -c Release`
5. **Run tests** - Verify all tests pass
6. **Deploy** - Push to production

---

## Monitoring in Production

1. **Health checks** - `GET /health` endpoint
2. **Application Insights** - Performance and errors
3. **MongoDB monitoring** - Query performance
4. **Log aggregation** - Centralized logging
5. **Alerts** - Set up error and performance alerts

---

## Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)

---

**Last Updated**: 2024  
**Version**: 1.0.0  
**Status**: Active Development
