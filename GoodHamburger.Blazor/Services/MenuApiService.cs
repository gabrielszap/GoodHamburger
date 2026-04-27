using GoodHamburger.Blazor.Models;

namespace GoodHamburger.Blazor.Services;

public sealed class MenuApiService
{
    private readonly ApiClient _apiClient;

    public MenuApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResult<ProductResult>> GetMenuAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _apiClient.GetAsync<PagedResult<ProductResult>>(
            $"/api/product?page={page}&pageSize={pageSize}",
            cancellationToken);

        return result ?? new PagedResult<ProductResult>();
    }
}
