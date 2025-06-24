using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CornerShop.Services;
using CornerShop.Models;
using Xunit;

namespace CornerShop.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        var loginRequest = new { Username = "admin", Password = "password" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(result.TryGetProperty("token", out var token));
        Assert.False(string.IsNullOrEmpty(token.GetString()));
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new { Username = "admin", Password = "wrongpassword" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_WithoutAuthentication_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Product>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Links);
    }

    [Fact]
    public async Task GetProducts_WithPaginationAndSorting_ReturnsCorrectResults()
    {
        // Arrange
        var url = "/api/v1/products?page=1&pageSize=5&sortBy=Name&sortOrder=asc";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Product>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Count() <= 5); // Page size limit
    }

    [Fact]
    public async Task GetProducts_WithSearchTerm_ReturnsFilteredResults()
    {
        // Arrange
        var url = "/api/v1/products?searchTerm=test";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Product>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task CreateProduct_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Category = "Test Category",
            Price = 10.99m,
            StockQuantity = 100
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/products", product);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_WithAuthentication_ReturnsCreated()
    {
        // Arrange
        var token = await GetAuthToken();
        var product = new Product
        {
            Name = "Test Product",
            Category = "Test Category",
            Price = 10.99m,
            StockQuantity = 100
        };

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/products", product);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(product.Name, result.Data.Name);
    }

    [Fact]
    public async Task GetStores_WithoutAuthentication_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/stores");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Store>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Links);
    }

    [Fact]
    public async Task GetStores_WithPaginationAndSorting_ReturnsCorrectResults()
    {
        // Arrange
        var url = "/api/v1/stores?page=1&pageSize=3&sortBy=Name&sortOrder=asc";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Store>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Count() <= 3); // Page size limit
    }

    [Fact]
    public async Task CreateStore_WithAuthentication_ReturnsCreated()
    {
        // Arrange
        var token = await GetAuthToken();
        var store = new Store
        {
            Name = "Test Store",
            Location = "Test Location",
            Address = "123 Test Street"
        };

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/stores", store);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Store>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(store.Name, result.Data.Name);
    }

    [Fact]
    public async Task GetSales_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/sales/store/test-store/recent");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSales_WithAuthentication_ReturnsOk()
    {
        // Arrange
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/v1/sales/store/test-store/recent");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Sale>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetReports_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/reports/sales/consolidated");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetReports_WithAuthentication_ReturnsOk()
    {
        // Arrange
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/v1/reports/sales/consolidated");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<ConsolidatedSalesReport>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task SearchProducts_WithEmptySearchTerm_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/products/search?searchTerm=");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(result);
        Assert.Equal(400, result.Status);
        Assert.Contains("Search term is required", result.Message);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/products/invalid-id");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(result);
        Assert.Equal(404, result.Status);
        Assert.Contains("not found", result.Message);
    }

    [Fact]
    public async Task UpdateProduct_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var product = new Product
        {
            Name = "Updated Product",
            Category = "Updated Category",
            Price = 15.99m
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/v1/products/test-id", product);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.DeleteAsync("/api/v1/products/test-id");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetSalesByDateRange_WithAuthentication_ReturnsOk()
    {
        // Arrange
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var startDate = DateTime.Today.AddDays(-7);
        var endDate = DateTime.Today;
        var url = $"/api/v1/sales/date-range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&page=1&pageSize=10&sortBy=Date&sortOrder=desc";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Sale>>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
    }

    private async Task<string> GetAuthToken()
    {
        var loginRequest = new { Username = "admin", Password = "password" };
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.GetProperty("token").GetString() ?? string.Empty;
        }

        return string.Empty;
    }
}

// API Response Models for testing
public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public List<Link> Links { get; set; } = new();
}

public class Link
{
    public string Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public DateTime Timestamp { get; set; }
    public int Status { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}

public class ConsolidatedSalesReport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageSaleAmount { get; set; }
    public List<StoreSalesReport> StoreReports { get; set; } = new();
}

public class StoreSalesReport
{
    public string StoreId { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageSaleAmount { get; set; }
}
