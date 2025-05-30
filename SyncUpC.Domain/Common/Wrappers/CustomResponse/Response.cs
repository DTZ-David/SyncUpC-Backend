using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SyncUpC.Domain.Common.Wrappers.CustomResponse;

public class Response<T> : ResponseGeneric<T>
{
    public Response() { }
    public Response(int statusCode, T? data) : base()
    {
        StatusCode = statusCode;
        IsSuccess = true;
        if (data != null)
        {
            Data = data;
        }
    }
}
