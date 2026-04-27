using GoodHamburger.Blazor.Models;

namespace GoodHamburger.Blazor.Services;

public sealed class OrderApiService
{
    private readonly ApiClient _apiClient;

    public OrderApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResult<OrderResult>> GetOrdersAsync(
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _apiClient.GetAsync<PagedResult<OrderResult>>(
            $"/api/order?page={page}&pageSize={pageSize}",
            cancellationToken);

        return result ?? new PagedResult<OrderResult>();
    }

    public async Task<OrderResult> CreateAsync(
        IReadOnlyCollection<Guid> productIds,
        CancellationToken cancellationToken = default)
    {
        var request = new OrderRequest
        {
            ProductIds = productIds
        };

        var result = await _apiClient.PostAsync<OrderRequest, OrderResult>(
            "/api/order",
            request,
            cancellationToken);

        return result ?? throw new InvalidOperationException("A API não retornou o pedido criado.");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _apiClient.DeleteAsync($"/api/order/{id}", cancellationToken);
    }
}
