using System.Net;
using System.Text.Json;
using candidato.Exceptions;

namespace candidato.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção não tratada.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = exception.Message,
            detailed = exception.InnerException?.Message
        };

        if (exception is ValidacaoException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response = new
            {
                error = "Ocorreu um erro interno no servidor.",
                detailed = (string)null!
            };
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
