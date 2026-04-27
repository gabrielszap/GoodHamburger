namespace GoodHamburger.Blazor.Models;

public sealed class ApiProblemDetails
{
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}