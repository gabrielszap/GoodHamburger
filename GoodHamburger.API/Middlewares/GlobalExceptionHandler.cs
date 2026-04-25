using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using GoodHamburger.Api.ErrorHandling;
using GoodHamburger.Application.Abstractions.Exceptions;
using GoodHamburger.Domain.Abstractions.Exceptions;

namespace GoodHamburger.Api.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation failed",
                validationException.Message),
            NotFoundException => (
                StatusCodes.Status404NotFound,
                "Resource not found",
                exception.Message),
            ConflictException => (
                StatusCodes.Status409Conflict,
                "Conflict",
                exception.Message),
            UnauthorizedException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                exception.Message),
            BadHttpRequestException => (
                StatusCodes.Status400BadRequest,
                "Bad request",
                exception.Message),
            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Bad request",
                exception.Message),
            DomainException => (
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                exception.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "An unexpected error occurred.")
        };

        _logger.LogError(
            exception,
            "Unhandled exception while processing {Method} {Path}. TraceId: {TraceId}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            Activity.Current?.Id ?? httpContext.TraceIdentifier);

        await ApiProblemDetailsWriter.WriteAsync(
            httpContext,
            statusCode,
            title,
            detail,
            exception,
            exception as ValidationException,
            cancellationToken);

        return true;
    }
}
