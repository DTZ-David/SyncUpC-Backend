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
            var message = ex.Message;
            var modelResponse = new Response<object>() { Message = message };

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case BusinessException error:
                    await error.HandleError(context, modelResponse);
                    break;
                case FluentValidationException error:
                    await error.HandleError(context, modelResponse);
                    break;
                default:
                    context.Response.StatusCode = (int)MessageStatusCode.ServerError;
                    modelResponse.StatusCode = (int)MessageStatusCode.ServerError;
                    _logger.LogError("{Message}", ex.StackTrace);
                    break;
            }

            await JsonSerializer.SerializeAsync(context.Response.Body, modelResponse, _jsonSerializerOptions);
        }
    }
}
