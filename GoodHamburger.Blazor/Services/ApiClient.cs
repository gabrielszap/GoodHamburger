using GoodHamburger.Blazor.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoodHamburger.Blazor.Services;

public sealed class ApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<T>(url, JsonOptions, cancellationToken);
    }

    public async Task<T?> PostAsync<TRequest, T>(
        string url,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, request, JsonOptions, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken);
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
    }

    public async Task DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken);
        }
    }

    private static async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>(
            JsonOptions,
            cancellationToken);

        if (problem?.Errors is not null && problem.Errors.Any())
        {
            var errors = string.Join(" | ", problem.Errors.Values.SelectMany(x => x));
            throw new ApiException(errors, problem.Status);
        }

        var message = problem?.Detail
            ?? problem?.Title
            ?? "Erro ao processar a requisição.";

        throw new ApiException(message, problem?.Status);
    }
}
