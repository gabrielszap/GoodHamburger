using GoodHamburger.Application.DTOs.Common;
using GoodHamburger.Application.DTOs.Product;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoodHamburger.IntegrationTests;

public sealed class IntegrationTestFixture
{
    public HttpClient Client { get; }

    public IntegrationTestFixture()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8080")
        };
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
    {
        new JsonStringEnumConverter()
    }
    };

    public async Task<List<ProductResult>> GetMenuProductsAsync()
    {
        var response = await Client.GetAsync("/api/product?page=1&pageSize=20");

        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductResult>>(JsonOptions);

        return result?.Items.ToList() ?? [];
    }
}