# API Contract Documentation

## Overview
This document defines the REST API contracts for the Azure Intelligent Supply Chain Management Platform.

---

## Company Management APIs

### 1. Create Company

**Endpoint**: `POST /api/v1/companies`

**Authentication**: Required (Bearer Token)

**Request Body**:
```json
{
  "name": "string (required, max 255)",
  "registrationNumber": "string (required, max 50)",
  "taxNumber": "string (required, max 50)",
  "companyType": "integer (1-5, required)",
  "description": "string (optional)",
  "industry": "string (optional)",
  "numberOfEmployees": "integer (optional, > 0)",
  "annualRevenue": "decimal (optional, > 0)",
  "headquarters": {
    "street": "string (required if provided)",
    "city": "string (required if provided)",
    "state": "string (required if provided)",
    "postalCode": "string (required if provided)",
    "country": "string (required if provided)"
  },
  "contact": {
    "email": "string (required if provided, valid email)",
    "phone": "string (required if provided, E.164 format)",
    "website": "string (optional, valid URL)"
  }
}
```

**Company Type Values**:
- 1 = Internal
- 2 = Supplier
- 3 = Customer
- 4 = Logistics
- 5 = Manufacturer

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Company created successfully",
  "data": {
    "id": "guid",
    "name": "string",
    "registrationNumber": "string",
    "taxNumber": "string",
    "description": "string",
    "headquarters": {
      "street": "string",
      "city": "string",
      "state": "string",
      "postalCode": "string",
      "country": "string"
    },
    "contact": {
      "email": "string",
      "phone": "string",
      "website": "string"
    },
    "status": 1,
    "companyType": 1,
    "industry": "string",
    "numberOfEmployees": 0,
    "annualRevenue": 0,
    "logoUrl": null,
    "bankAccounts": [],
    "createdAt": "2024-01-15T10:00:00Z",
    "createdBy": "string",
    "updatedAt": null,
    "updatedBy": null,
    "isDeleted": false,
    "deletedAt": null,
    "version": 1
  }
}
```

**Error Responses**:

- 400 Bad Request (Validation Error):
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "Name": ["Company name is required"],
    "Email": ["Email must be a valid email address"]
  }
}
```

- 401 Unauthorized:
```json
{
  "success": false,
  "message": "Unauthorized"
}
```

---

### 2. Get Company by ID

**Endpoint**: `GET /api/v1/companies/{id}`

**Authentication**: Required (Bearer Token)

**Path Parameters**:
- `id` (string, UUID): Company ID

**Response** (200 OK):
```json
{
  "success": true,
  "message": null,
  "data": {
    "id": "guid",
    "name": "string",
    "registrationNumber": "string",
    "taxNumber": "string",
    "description": "string",
    "headquarters": {...},
    "contact": {...},
    "status": 1,
    "companyType": 1,
    "industry": "string",
    "numberOfEmployees": 0,
    "annualRevenue": 0,
    "logoUrl": null,
    "bankAccounts": [],
    "createdAt": "2024-01-15T10:00:00Z",
    "createdBy": "string",
    "updatedAt": null,
    "updatedBy": null,
    "isDeleted": false,
    "deletedAt": null,
    "version": 1
  }
}
```

**Error Responses**:

- 404 Not Found:
```json
{
  "success": false,
  "message": "Company with ID 'xxx' not found"
}
```

- 401 Unauthorized

---

### 3. Get All Companies (Paginated)

**Endpoint**: `GET /api/v1/companies`

**Authentication**: Required (Bearer Token)

**Query Parameters**:
- `pageNumber` (integer, optional, default: 1): Page number (1-based)
- `pageSize` (integer, optional, default: 10, max: 100): Items per page
- `statusFilter` (integer, optional): Filter by status (1-4)
- `companyTypeFilter` (integer, optional): Filter by type (1-5)
- `searchTerm` (string, optional): Search by name or registration number

**Response** (200 OK):
```json
{
  "success": true,
  "message": null,
  "data": {
    "pageNumber": 1,
    "pageSize": 10,
    "totalItems": 25,
    "totalPages": 3,
    "items": [
      {
        "id": "guid",
        "name": "string",
        "registrationNumber": "string",
        "taxNumber": "string",
        "description": "string",
        "headquarters": {...},
        "contact": {...},
        "status": 1,
        "companyType": 1,
        "industry": "string",
        "numberOfEmployees": 0,
        "annualRevenue": 0,
        "logoUrl": null,
        "bankAccounts": [],
        "createdAt": "2024-01-15T10:00:00Z",
        "createdBy": "string",
        "updatedAt": null,
        "updatedBy": null,
        "isDeleted": false,
        "deletedAt": null,
        "version": 1
      }
    ],
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

**Status Filter Values**:
- 1 = Active
- 2 = Inactive
- 3 = Suspended
- 4 = Deleted

---

### 4. Update Company

**Endpoint**: `PUT /api/v1/companies/{id}`

**Authentication**: Required (Bearer Token)

**Path Parameters**:
- `id` (string, UUID): Company ID

**Request Body**:
```json
{
  "name": "string (required, max 255)",
  "description": "string (optional)",
  "industry": "string (optional)",
  "numberOfEmployees": "integer (optional, > 0)",
  "annualRevenue": "decimal (optional, > 0)",
  "logoUrl": "string (optional, valid URL)",
  "headquarters": {
    "street": "string (required if provided)",
    "city": "string (required if provided)",
    "state": "string (required if provided)",
    "postalCode": "string (required if provided)",
    "country": "string (required if provided)"
  },
  "contact": {
    "email": "string (required if provided, valid email)",
    "phone": "string (required if provided, E.164 format)",
    "website": "string (optional, valid URL)"
  }
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Company updated successfully",
  "data": {
    "id": "guid",
    "name": "string",
    "registrationNumber": "string",
    "taxNumber": "string",
    "description": "string",
    "headquarters": {...},
    "contact": {...},
    "status": 1,
    "companyType": 1,
    "industry": "string",
    "numberOfEmployees": 0,
    "annualRevenue": 0,
    "logoUrl": null,
    "bankAccounts": [],
    "createdAt": "2024-01-15T10:00:00Z",
    "createdBy": "string",
    "updatedAt": "2024-01-15T10:30:00Z",
    "updatedBy": "string",
    "isDeleted": false,
    "deletedAt": null,
    "version": 2
  }
}
```

**Error Responses**:

- 400 Bad Request
- 404 Not Found
- 422 Unprocessable Entity (Business Rule Violation):
```json
{
  "success": false,
  "message": "Company is already active."
}
```

---

### 5. Delete Company (Soft Delete)

**Endpoint**: `DELETE /api/v1/companies/{id}`

**Authentication**: Required (Bearer Token)

**Path Parameters**:
- `id` (string, UUID): Company ID

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Company deleted successfully"
}
```

**Error Responses**:

- 400 Bad Request:
```json
{
  "success": false,
  "message": "Company with ID 'xxx' not found"
}
```

- 401 Unauthorized

---

## System APIs

### Health Check

**Endpoint**: `GET /health`

**Authentication**: Not Required

**Response** (200 OK - Healthy):
```json
{
  "status": "healthy",
  "timestamp": "2024-01-15T10:00:00Z"
}
```

**Response** (503 Service Unavailable - Unhealthy):
```json
{
  "status": "unhealthy",
  "timestamp": "2024-01-15T10:00:00Z"
}
```

---

### API Root

**Endpoint**: `GET /`

**Authentication**: Not Required

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Azure Intelligent Supply Chain Management Platform API"
}
```

---

## Error Handling

### Standard Error Response Format

```json
{
  "success": false,
  "message": "Error message describing what went wrong",
  "errors": {
    "fieldName": ["Error message 1", "Error message 2"]
  },
  "traceId": "0HN1GHEFG4S79:00000001"
}
```

### HTTP Status Codes

| Status | Meaning | Use Case |
|--------|---------|----------|
| 200 | OK | Successful GET, PUT operations |
| 201 | Created | Successful POST operation |
| 400 | Bad Request | Validation errors, malformed request |
| 401 | Unauthorized | Missing or invalid authentication |
| 403 | Forbidden | User lacks required permissions |
| 404 | Not Found | Resource doesn't exist |
| 422 | Unprocessable Entity | Business rule violation |
| 500 | Internal Server Error | Unexpected server error |
| 503 | Service Unavailable | Database or dependent service down |

---

## Validation Rules

### Company Fields

| Field | Rules |
|-------|-------|
| name | Required, 1-255 characters |
| registrationNumber | Required, 1-50 characters |
| taxNumber | Required, 1-50 characters |
| companyType | Required, integer 1-5 |
| numberOfEmployees | Optional, must be > 0 if provided |
| annualRevenue | Optional, must be > 0 if provided |

### Address Fields

| Field | Rules |
|-------|-------|
| street | Required, 1-255 characters |
| city | Required, 1-100 characters |
| state | Required, 1-100 characters |
| postalCode | Required, 1-20 characters |
| country | Required, 1-100 characters |

### Contact Fields

| Field | Rules |
|-------|-------|
| email | Required, valid email format |
| phone | Required, E.164 format (+1234567890) |
| website | Optional, valid URL if provided |

---

## Authentication

### JWT Token

The API uses JWT (JSON Web Token) authentication via Bearer tokens.

**Header Format**:
```
Authorization: Bearer <your-jwt-token>
```

**Token Claims** (structure):
```json
{
  "sub": "user-id",
  "email": "user@example.com",
  "iss": "AzureIntelligentSupplyChain",
  "aud": "AzureIntelligentSupplyChainAPI",
  "exp": 1234567890
}
```

---

## Rate Limiting

Currently not implemented. Will be added in future versions.

---

## Pagination

All list endpoints support pagination with the following query parameters:

- `pageNumber`: 1-based page number (default: 1)
- `pageSize`: Number of items per page (default: 10, max: 100)

**Response Structure**:
```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalItems": 50,
  "totalPages": 5,
  "items": [...],
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

---

## Versioning

Current API Version: **v1**

Future versions will use the path: `/api/v2/`, `/api/v3/`, etc.

---

## Changelog

### Version 1.0.0 (2024)
- Initial release
- Company Management CRUD operations
- Pagination support
- Filtering and search
- Validation
- Error handling
- JWT Authentication (ready)

---

## Examples

### Create Company Example

```bash
curl -X POST "http://localhost:5000/api/v1/companies" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "name": "TechCorp Solutions",
    "registrationNumber": "REG-2024-001",
    "taxNumber": "TAX-2024-001",
    "companyType": 2,
    "description": "Leading technology supplier",
    "industry": "Technology",
    "numberOfEmployees": 250,
    "annualRevenue": 25000000,
    "headquarters": {
      "street": "456 Tech Avenue",
      "city": "San Francisco",
      "state": "CA",
      "postalCode": "94105",
      "country": "USA"
    },
    "contact": {
      "email": "sales@techcorp.com",
      "phone": "+14155551234",
      "website": "https://techcorp.com"
    }
  }'
```

**Successful Response (201)**:
```json
{
  "success": true,
  "message": "Company created successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "TechCorp Solutions",
    "registrationNumber": "REG-2024-001",
    "taxNumber": "TAX-2024-001",
    "status": 1,
    "companyType": 2,
    "createdAt": "2024-01-15T10:00:00Z",
    "version": 1
  }
}
```

### List Companies Example

```bash
curl -X GET "http://localhost:5000/api/v1/companies?pageNumber=1&pageSize=20&companyTypeFilter=2&searchTerm=tech" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

**Last Updated**: 2024  
**API Version**: 1.0.0  
**Status**: Production Ready
