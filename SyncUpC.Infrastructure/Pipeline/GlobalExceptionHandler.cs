using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SyncUpC.Infraestructure.Pipeline;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        var modelResponse = new Response<object>();

        switch (ex)
        {
            case BusinessException businessEx:
                context.Response.StatusCode = businessEx.StatusCode;
                modelResponse.StatusCode = businessEx.StatusCode;
                modelResponse.IsSuccess = false;
                modelResponse.Message = businessEx.Message;
                break;

            case FluentValidationException validationEx:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                modelResponse.StatusCode = StatusCodes.Status400BadRequest;
                modelResponse.IsSuccess = false;
                modelResponse.Message = "Errores de validación";
                modelResponse.Errors = validationEx.Errors;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                modelResponse.StatusCode = StatusCodes.Status500InternalServerError;
                modelResponse.IsSuccess = false;
                modelResponse.Message = "Error interno del servidor";
                _logger.LogError(ex, "Unhandled exception occurred");
                break;
        }

        var json = JsonSerializer.Serialize(modelResponse, _jsonSerializerOptions);
        await context.Response.WriteAsync(json);
    }
}