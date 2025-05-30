using Microsoft.AspNetCore.Http;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Domain.Common.Exceptions;


public class BusinessException : Exception
{
    private int StatusCode { get; set; }
    public BusinessException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public virtual Task HandleError(HttpContext context, Response<object> response)
    {
        context.Response.StatusCode = StatusCode;
        response.StatusCode = StatusCode;
        return Task.CompletedTask;
    }
}
