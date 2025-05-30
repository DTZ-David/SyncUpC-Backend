using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Common.Wrappers.CustomResponse;

public class ResponseGeneric<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public List<string>? Errors { get; set; }
}
