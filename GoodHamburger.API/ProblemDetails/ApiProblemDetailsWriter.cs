using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Api.ErrorHandling;

internal static class ApiProblemDetailsWriter
{
    public static async Task WriteAsync(
        HttpContext httpContext,
        int statusCode,
        string title,
        string detail,
        Exception? exception = null,
        ValidationException? validationException = null,
        CancellationToken cancellationToken = default)
    {
        httpContext.Response.StatusCode = statusCode;

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        var problemDetails = validationException is null
            ? new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}"
            }
            : new ValidationProblemDetails(
                validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).ToArray()))
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}"
            };

        problemDetails.Extensions["traceId"] = traceId;

        var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
        var wrote = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });

        if (!wrote)
        {
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
    }
}
