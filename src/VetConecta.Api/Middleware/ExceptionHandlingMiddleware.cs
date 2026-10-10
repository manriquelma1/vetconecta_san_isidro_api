using System.Net;
using System.Text.Json;
using FluentValidation;
using VetConecta.Domain.Exceptions;

namespace VetConecta.Api.Middleware;

/// <summary>Traduce las excepciones de Application/Domain a codigos HTTP, en un solo lugar.</summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, mensaje, errores) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                "Uno o más campos no son válidos.",
                validationEx.Errors.Select(e => new { campo = e.PropertyName, error = e.ErrorMessage })),

            NotFoundException notFoundEx => (
                HttpStatusCode.NotFound, notFoundEx.Message, null),

            ForbiddenAccessException forbiddenEx => (
                HttpStatusCode.Forbidden, forbiddenEx.Message, null),

            UnauthorizedAccessException unauthorizedEx => (
                HttpStatusCode.Unauthorized, unauthorizedEx.Message, null),

            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.", null)
        };

        if (status == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "Error no controlado");
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new { mensaje, errores }));
    }
}
