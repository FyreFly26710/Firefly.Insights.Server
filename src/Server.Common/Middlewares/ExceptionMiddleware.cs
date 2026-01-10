using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Server.Common.Types;
using System.Text.Json;

namespace Server.Common.Middlewares;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            HandleException(context, ex);
        }
    }

    private void HandleException(HttpContext context, Exception ex)
    {
        int statusCode;
        string message;

        if (ex is HandledException he)
        {
            statusCode = he.StatusCode;
            message = he.Message;
            _logger.LogWarning(ex, "Handled exception: {Message}", message);
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            message = $"An unhandled server error occurred: {ex.Message}";
            _logger.LogError(ex, "Unhandled exception: {Message}", message);
        }

        var error = new ErrorObj
        {
            Message = message,
            Code = statusCode
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(error);

        context.Response.WriteAsync(result);
    }
    class ErrorObj
    {
        public string Message { get; set; } = "";
        public int Code { get; set; }
    }
}
