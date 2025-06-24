
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
