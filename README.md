# Corner Shop Multi-Store Management System

A modern, distributed point-of-sale and inventory management system for multi-store businesses. Each store operates independently with a local SQLite database, while the head office consolidates data using a central MongoDB database. The system is designed for reliability, offline support, and easy synchronization.

## Features
- **Multi-store support**: Each store has its own local SQLite database for products and sales.
- **Offline operation**: Stores can operate independently, even without internet connectivity.
- **Centralized reporting**: The head office uses MongoDB to view consolidated sales and stock across all stores.
- **One-click sync**: Admins can synchronize all stores' local data to the central database from the Reports page.
- **Modern web interface**: Built with ASP.NET Core MVC for a responsive, user-friendly experience.
- **REST API**: Full REST-compliant API with versioning, HATEOAS, and standardized error handling.
- **Cross-Origin Support**: CORS-enabled for frontend integration.

## REST API Features
- **API Versioning**: All endpoints use `/api/v1/` prefix for future compatibility
- **HATEOAS**: Hypermedia links in all responses for navigation
- **Standardized Error Responses**: Consistent error format with timestamp, status, and path
- **HTTP Status Codes**: Proper use of 200, 201, 204, 400, 404, 500
- **Caching Headers**: Response caching with appropriate durations
- **Content Negotiation**: Support for JSON and XML formats via Accept header
- **PATCH Support**: Partial updates for all resources
- **OpenAPI 3.0 Documentation**: Complete Swagger/OpenAPI documentation

## Architecture
- **Web Client**: ASP.NET Core MVC web application
- **REST API**: ASP.NET Core Web API with full REST compliance
- **Store Data Layer**: Local SQLite database per store
- **Central Data Layer**: MongoDB for consolidated data and reporting
- **Sync Service**: Pushes local sales to MongoDB for global reporting

## Getting Started
1. Clone the repository
2. Run the application with `dotnet run`
3. On store creation, a new SQLite database file is created for that store
4. Use the Reports page to sync all stores to MongoDB for consolidated reporting

## API Documentation
- **Swagger UI**: [http://localhost:5000/api-docs](http://localhost:5000/api-docs) - Interactive API documentation
- **ReDoc UI**: [http://localhost:5000/redoc](http://localhost:5000/redoc) - Alternative documentation viewer
- **API Documentation Page**: [http://localhost:5000/Home/ApiDocumentation](http://localhost:5000/Home/ApiDocumentation) - Detailed API guide

## API Endpoints
### Products API (`/api/v1/products`)
- `GET /api/v1/products` - Get all products
- `GET /api/v1/products/{id}` - Get specific product
- `GET /api/v1/products/store/{storeId}` - Get products by store
- `GET /api/v1/products/search` - Search products
- `GET /api/v1/products/low-stock` - Get low stock products
- `POST /api/v1/products` - Create new product
- `PUT /api/v1/products/{id}` - Update product (full update)
- `PATCH /api/v1/products/{id}` - Partially update product
- `DELETE /api/v1/products/{id}` - Delete product

### Stores API (`/api/v1/stores`)
- `GET /api/v1/stores` - Get all stores
- `GET /api/v1/stores/{id}` - Get specific store
- `GET /api/v1/stores/search` - Search stores
- `POST /api/v1/stores` - Create new store
- `PUT /api/v1/stores/{id}` - Update store (full update)
- `PATCH /api/v1/stores/{id}` - Partially update store
- `DELETE /api/v1/stores/{id}` - Delete store

### Sales API (`/api/v1/sales`)
- `GET /api/v1/sales/store/{storeId}/recent` - Get recent sales for store
- `GET /api/v1/sales/{id}` - Get specific sale
- `GET /api/v1/sales/{id}/details` - Get sale details with items
- `GET /api/v1/sales/date-range` - Get sales by date range
- `POST /api/v1/sales` - Create new sale
- `POST /api/v1/sales/{id}/cancel` - Cancel sale
- `PATCH /api/v1/sales/{id}` - Partially update sale

### Reports API (`/api/v1/reports`)
- `GET /api/v1/reports/sales/consolidated` - Get consolidated sales report
- `GET /api/v1/reports/inventory` - Get inventory report
- `GET /api/v1/reports/products/top-selling` - Get top selling products
- `GET /api/v1/reports/sales/trend` - Get sales trend report

## CORS Testing
- **CORS Test Page**: [http://localhost:5000/cors-test.html](http://localhost:5000/cors-test.html) - Test cross-origin requests

## Documentation
- [Technical Docs](docs/README.md)
- [UML Diagrams](docs/UML/)
- [Architecture Decision Records](docs/ADR/)
- [API Documentation](docs/API_README.md)
- [CORS Configuration](docs/CORS_README.md)

## License
MIT