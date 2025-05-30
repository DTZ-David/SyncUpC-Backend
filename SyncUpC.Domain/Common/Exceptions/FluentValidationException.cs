using Microsoft.AspNetCore.Http;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Common.Exceptions;

public class FluentValidationException : Exception
{
    public List<string> Errors { get; }
    public int StatusCode { get; }

    public FluentValidationException() : base()
    {
        Errors = new List<string>();
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
    public FluentValidationException(IEnumerable<string> failures) : this()
    {
        Errors = failures.ToList();
    }
    public virtual Task HandleError(HttpContext context, Response<object> response)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.Errors = Errors;
        return Task.CompletedTask;
    }
}
