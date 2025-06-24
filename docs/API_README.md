# CornerShop REST API Documentation

## Overview

The CornerShop REST API provides programmatic access to all CornerShop functionality with full REST compliance. The API follows OpenAPI 3.0 specification and includes comprehensive documentation, examples, and testing capabilities.

## Base URL

All API endpoints are prefixed with `/api/v1/` for versioning support.

```
https://localhost:5000/api/v1/
```

## REST Compliance

The API implements all REST principles and constraints:

### REST Principles
- **Stateless**: Each request contains all necessary information
- **Cacheable**: Responses include appropriate cache headers
- **Uniform Interface**: Standard HTTP methods and URI patterns
- **Layered System**: API abstracts underlying complexity
- **HATEOAS**: Hypermedia links in all responses

### API Features
- **Versioning**: All endpoints use `/api/v1/` prefix
- **HATEOAS**: Navigation links in all responses
- **Standardized Errors**: Consistent error format with timestamp, status, and path
- **HTTP Status Codes**: Proper use of 200, 201, 204, 400, 404, 500
- **Caching**: Response caching with appropriate durations
- **Content Negotiation**: Support for JSON and XML formats
- **PATCH Support**: Partial updates for all resources

## Authentication

Currently, the API does not require authentication. All endpoints are publicly accessible.

## Content Types

The API supports multiple content types via the `Accept` header:

- `application/json` (default)
- `application/xml`

### Example
```bash
curl -H "Accept: application/xml" https://localhost:5000/api/v1/products
```

## Response Format

All successful responses follow a standardized format with HATEOAS links:

```json
{
  "data": { /* actual response data */ },
  "links": [
    {
      "href": "/api/v1/products",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1/products",
      "rel": "create",
      "method": "POST"
    }
  ],
  "timestamp": "2025-01-27T10:30:00Z"
}
```

## Error Response Format

All error responses follow a standardized format:

```json
{
  "timestamp": "2025-01-27T10:30:00Z",
  "status": 400,
  "error": "Bad Request",
  "message": "Search term is required",
  "path": "/api/v1/products/search"
}
```

## HTTP Status Codes

- **200 OK**: Request successful
- **201 Created**: Resource created successfully
- **204 No Content**: Request successful, no content to return
- **400 Bad Request**: Invalid request data
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

## Caching

The API implements response caching with appropriate durations:

- **Products/Stores**: 5 minutes cache
- **Sales**: 1-5 minutes cache (depending on endpoint)
- **Reports**: 5-10 minutes cache
- **Search results**: 1 minute cache

## API Endpoints

### Products API

Base URL: `/api/v1/products`

#### Get All Products
```http
GET /api/v1/products
```

**Response**: List of all products with HATEOAS links

#### Get Product by ID
```http
GET /api/v1/products/{id}
```

**Parameters**:
- `id` (string): Product ID

**Response**: Product details with navigation links

#### Get Products by Store
```http
GET /api/v1/products/store/{storeId}
```

**Parameters**:
- `storeId` (string): Store ID

**Response**: List of products for the specified store

#### Search Products
```http
GET /api/v1/products/search?searchTerm={term}&storeId={id}
```

**Parameters**:
- `searchTerm` (string, required): Search term
- `storeId` (string, optional): Filter by store ID

**Response**: List of matching products

#### Get Low Stock Products
```http
GET /api/v1/products/low-stock?storeId={id}
```

**Parameters**:
- `storeId` (string, optional): Filter by store ID

**Response**: List of products with low stock

#### Create Product
```http
POST /api/v1/products
Content-Type: application/json
```

**Request Body**:
```json
{
  "name": "Sample Product",
  "category": "Electronics",
  "price": 99.99,
  "storeId": "store-id-here",
  "stockQuantity": 50,
  "minimumStockLevel": 10,
  "reorderPoint": 5
}
```

**Response**: Created product with navigation links (201 Created)

#### Update Product (Full Update)
```http
PUT /api/v1/products/{id}
Content-Type: application/json
```

**Parameters**:
- `id` (string): Product ID

**Request Body**: Complete product object

**Response**: No content (204 No Content)

#### Partially Update Product
```http
PATCH /api/v1/products/{id}
Content-Type: application/json
```

**Parameters**:
- `id` (string): Product ID

**Request Body**:
```json
{
  "name": "Updated Product Name",
  "price": 89.99
}
```

**Response**: No content (204 No Content)

#### Delete Product
```http
DELETE /api/v1/products/{id}?storeId={storeId}
```

**Parameters**:
- `id` (string): Product ID
- `storeId` (string, optional): Store ID for validation

**Response**: No content (204 No Content)

### Stores API

Base URL: `/api/v1/stores`

#### Get All Stores
```http
GET /api/v1/stores
```

**Response**: List of all stores with HATEOAS links

#### Get Store by ID
```http
GET /api/v1/stores/{id}
```

**Parameters**:
- `id` (string): Store ID

**Response**: Store details with navigation links

#### Search Stores
```http
GET /api/v1/stores/search?searchTerm={term}
```

**Parameters**:
- `searchTerm` (string, required): Search term

**Response**: List of matching stores

#### Create Store
```http
POST /api/v1/stores
Content-Type: application/json
```

**Request Body**:
```json
{
  "name": "Sample Store",
  "location": "Downtown",
  "address": "123 Main St",
  "isHeadquarters": false,
  "status": "Active"
}
```

**Response**: Created store with navigation links (201 Created)

#### Update Store (Full Update)
```http
PUT /api/v1/stores/{id}
Content-Type: application/json
```

**Parameters**:
- `id` (string): Store ID

**Request Body**: Complete store object

**Response**: No content (204 No Content)

#### Partially Update Store
```http
PATCH /api/v1/stores/{id}
Content-Type: application/json
```

**Parameters**:
- `id` (string): Store ID

**Request Body**:
```json
{
  "name": "Updated Store Name",
  "status": "Inactive"
}
```

**Response**: No content (204 No Content)

#### Delete Store
```http
DELETE /api/v1/stores/{id}
```

**Parameters**:
- `id` (string): Store ID

**Response**: No content (204 No Content)

### Sales API

Base URL: `/api/v1/sales`

#### Get Recent Sales for Store
```http
GET /api/v1/sales/store/{storeId}/recent?limit={number}
```

**Parameters**:
- `storeId` (string): Store ID
- `limit` (integer, optional): Number of recent sales (default: 10)

**Response**: List of recent sales

#### Get Sale by ID
```http
GET /api/v1/sales/{id}
```

**Parameters**:
- `id` (string): Sale ID

**Response**: Sale details with navigation links

#### Get Sale Details
```http
GET /api/v1/sales/{id}/details
```

**Parameters**:
- `id` (string): Sale ID

**Response**: Sale details with items and store information

#### Get Sales by Date Range
```http
GET /api/v1/sales/date-range?startDate={date}&endDate={date}&storeId={id}
```

**Parameters**:
- `startDate` (datetime): Start date
- `endDate` (datetime): End date
- `storeId` (string, optional): Filter by store ID

**Response**: List of sales in the date range

#### Create Sale
```http
POST /api/v1/sales
Content-Type: application/json
```

**Request Body**:
```json
{
  "storeId": "store-id-here",
  "items": [
    {
      "productName": "Sample Product",
      "quantity": 2,
      "unitPrice": 99.99
    }
  ]
}
```

**Response**: Created sale with navigation links (201 Created)

#### Cancel Sale
```http
POST /api/v1/sales/{id}/cancel?storeId={storeId}
```

**Parameters**:
- `id` (string): Sale ID
- `storeId` (string): Store ID

**Response**: No content (204 No Content)

#### Partially Update Sale
```http
PATCH /api/v1/sales/{id}
Content-Type: application/json
```

**Parameters**:
- `id` (string): Sale ID

**Request Body**:
```json
{
  "status": "Cancelled",
  "totalAmount": 199.98
}
```

**Response**: No content (204 No Content)

### Reports API

Base URL: `/api/v1/reports`

#### Get Consolidated Sales Report
```http
GET /api/v1/reports/sales/consolidated?startDate={date}&endDate={date}
```

**Parameters**:
- `startDate` (datetime, optional): Start date for report
- `endDate` (datetime, optional): End date for report

**Response**: Consolidated sales report across all stores

#### Get Inventory Report
```http
GET /api/v1/reports/inventory
```

**Response**: Inventory status report across all stores

#### Get Top Selling Products
```http
GET /api/v1/reports/products/top-selling?limit={number}&storeId={id}
```

**Parameters**:
- `limit` (integer, optional): Number of top products (default: 10)
- `storeId` (string, optional): Filter by store ID

**Response**: Top selling products report

#### Get Sales Trend Report
```http
GET /api/v1/reports/sales/trend?period={daily|weekly|monthly}&startDate={date}&endDate={date}
```

**Parameters**:
- `period` (string): Period grouping (daily, weekly, monthly)
- `startDate` (datetime, optional): Start date
- `endDate` (datetime, optional): End date

**Response**: Sales trend report

## Examples

### Creating a Product
```bash
curl -X POST "https://localhost:5000/api/v1/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop",
    "category": "Electronics",
    "price": 999.99,
    "storeId": "store-123",
    "stockQuantity": 10,
    "minimumStockLevel": 2,
    "reorderPoint": 1
  }'
```

### Searching Products
```bash
curl "https://localhost:5000/api/v1/products/search?searchTerm=laptop&storeId=store-123"
```

### Creating a Sale
```bash
curl -X POST "https://localhost:5000/api/v1/sales" \
  -H "Content-Type: application/json" \
  -d '{
    "storeId": "store-123",
    "items": [
      {
        "productName": "Laptop",
        "quantity": 1,
        "unitPrice": 999.99
      }
    ]
  }'
```

### Getting Consolidated Report
```bash
curl "https://localhost:5000/api/v1/reports/sales/consolidated?startDate=2025-01-01&endDate=2025-01-31"
```

## Testing

### Interactive Documentation
- **Swagger UI**: [http://localhost:5000/api-docs](http://localhost:5000/api-docs)
- **ReDoc UI**: [http://localhost:5000/redoc](http://localhost:5000/redoc)

### CORS Testing
- **CORS Test Page**: [http://localhost:5000/cors-test.html](http://localhost:5000/cors-test.html)

### Command Line Testing
```bash
# Test basic connectivity
curl https://localhost:5000/api/v1/products

# Test with different content type
curl -H "Accept: application/xml" https://localhost:5000/api/v1/products

# Test error handling
curl https://localhost:5000/api/v1/products/nonexistent-id
```

## Rate Limiting

Currently, no rate limiting is implemented. Consider implementing rate limiting for production use.

## Security Considerations

- All endpoints are publicly accessible (no authentication required)
- Consider implementing authentication and authorization for production use
- CORS is configured to allow cross-origin requests
- Input validation is performed on all endpoints

## Versioning Strategy

- Current version: `v1`
- All endpoints are prefixed with `/api/v1/`
- Future versions will use `/api/v2/`, `/api/v3/`, etc.
- Backward compatibility will be maintained within major versions

## Support

For API support and questions:
- Check the interactive documentation at `/api-docs`
- Review the API documentation page at `/Home/ApiDocumentation`
- Test CORS functionality at `/cors-test.html` 