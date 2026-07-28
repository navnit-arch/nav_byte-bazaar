# Quick Start Guide - Azure Intelligent Supply Chain Platform

## Prerequisites

- .NET 8 SDK installed
- MongoDB (local or Atlas)
- Visual Studio 2022 or VS Code with C# extension
- Git

## Environment Setup

### 1. MongoDB Configuration

Update `appsettings.json` with your MongoDB connection:

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb+srv://username:password@cluster.mongodb.net/?retryWrites=true&w=majority",
    "DatabaseName": "AzureIntelligentSupplyChain",
    "InitializeOnStartup": true
  }
}
```

For local MongoDB:
```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "AzureIntelligentSupplyChain_Dev",
    "InitializeOnStartup": true
  }
}
```

### 2. JWT Secret Configuration

Update `appsettings.json`:

```json
{
  "Authentication": {
    "Issuer": "AzureIntelligentSupplyChain",
    "Audience": "AzureIntelligentSupplyChainAPI",
    "SecretKey": "your-very-long-secret-key-min-32-characters-here",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

## Building the Application

```bash
# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Run tests (when ready)
dotnet test
```

## Running the Application

### Development Mode
```bash
dotnet run --launch-profile https
```

The application will start at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

## Accessing the API

### Swagger/OpenAPI Documentation
```
https://localhost:5001/swagger/index.html
```

### Health Check Endpoint
```
GET https://localhost:5001/health
```

Response:
```json
{
  "status": "healthy",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## API Usage Examples

### Create a Company

```bash
curl -X POST "https://localhost:5001/api/v1/companies" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <JWT_TOKEN>" \
  -d '{
    "name": "Acme Corporation",
    "registrationNumber": "REG-12345",
    "taxNumber": "TAX-67890",
    "companyType": 1,
    "description": "Leading supply chain company",
    "industry": "Manufacturing",
    "numberOfEmployees": 500,
    "annualRevenue": 50000000.00,
    "headquarters": {
      "street": "123 Main St",
      "city": "New York",
      "state": "NY",
      "postalCode": "10001",
      "country": "USA"
    },
    "contact": {
      "email": "info@acme.com",
      "phone": "+12125551234",
      "website": "https://acme.com"
    }
  }'
```

### Get Company by ID

```bash
curl -X GET "https://localhost:5001/api/v1/companies/{id}" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

### List Companies (Paginated)

```bash
curl -X GET "https://localhost:5001/api/v1/companies?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

### Filter Companies by Type

```bash
curl -X GET "https://localhost:5001/api/v1/companies?companyTypeFilter=2" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

Company Type Codes:
- 1: Internal
- 2: Supplier
- 3: Customer
- 4: Logistics
- 5: Manufacturer

### Update Company

```bash
curl -X PUT "https://localhost:5001/api/v1/companies/{id}" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <JWT_TOKEN>" \
  -d '{
    "name": "Updated Company Name",
    "description": "Updated description"
  }'
```

### Delete Company (Soft Delete)

```bash
curl -X DELETE "https://localhost:5001/api/v1/companies/{id}" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

## JWT Authentication

To use the API, you need a JWT token. Currently, the authentication is set up but you'll need to implement the token generation endpoint or use a third-party identity provider.

### Getting Started with Authentication

1. Implement an authentication controller
2. Generate JWT tokens
3. Use token in Authorization header: `Authorization: Bearer <token>`

## Project Structure

- `src/Core/` - Domain and application layers
- `src/Infrastructure/` - Data access and external services
- `src/Modules/` - Feature modules (Company Management, etc.)
- `Program.cs` - Application entry point and configuration
- `appsettings.json` - Configuration

## Common Errors & Solutions

### MongoDB Connection Error
```
Error: "Unable to connect to MongoDB"
```
**Solution**: Verify MongoDB is running and connection string is correct in `appsettings.json`

### Invalid JWT Token
```
Error: "The token is invalid or expired"
```
**Solution**: Ensure you're using a valid JWT token in the Authorization header

### Validation Error
```
{
  "success": false,
  "message": "Validation failed",
  "errors": {...}
}
```
**Solution**: Review error details and adjust request payload according to validation rules

## Logging

Logs are written to:
- Console (development)
- File: `logs/app-<date>.txt` (all environments)
- Application Insights (if configured)

Enable detailed logging by updating `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

## Database Schema

The application automatically uses MongoDB collections:

- **companies** - Company master data with all details

### MongoDB Document Example
```json
{
  "_id": "ObjectId",
  "name": "Acme Corporation",
  "registrationNumber": "REG-12345",
  "taxNumber": "TAX-67890",
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
    "email": "info@acme.com",
    "phone": "+12125551234",
    "website": "https://acme.com"
  },
  "isDeleted": false,
  "version": 1,
  "createdAt": "2024-01-15T10:00:00Z",
  "createdBy": "user-id"
}
```

## Deployment

### Docker

Create a `Dockerfile`:

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

Build and run:
```bash
docker build -t supply-chain-api .
docker run -p 5000:80 supply-chain-api
```

### Azure App Service

```bash
az webapp create --resource-group <group> --plan <plan> --name <app-name> --runtime "DOTNET|8.0"
az webapp deployment source config-zip --resource-group <group> --name <app-name> --src-path publish.zip
```

## Next Steps

1. **Authentication**: Implement token generation and refresh logic
2. **Database Indexing**: Create MongoDB indexes for performance
3. **Unit Tests**: Add comprehensive test coverage
4. **Additional Modules**: Implement warehouse, inventory, and supplier modules
5. **AI Integration**: Add semantic kernel and AI agents

## Support & Documentation

- **API Docs**: https://localhost:5001/swagger
- **Code Comments**: Inline XML documentation
- **Architecture**: See `IMPLEMENTATION_SUMMARY.md`

## Version

- Platform: .NET 8
- API Version: v1
- Status: Production Ready (Phase 1)

---

**Last Updated**: 2024  
**Maintained by**: Architecture Team
