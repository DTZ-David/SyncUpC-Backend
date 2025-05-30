using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Pipeline;

public class SecurityHeader : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        context.Response.Headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
        context.Response.Headers.ContentSecurityPolicy =
            "default-src 'self'; " +
            "img-src 'self' data: https://cdn.jsdelivr.net; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net; " +
            "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
            "connect-src 'self'; " +
            "frame-src 'self';";

        context.Response.Headers["Permissions-Policy"] = "geolocation=(self), microphone=()";
        context.Response.Headers.XFrameOptions = "DENY";
        context.Response.Headers.XContentTypeOptions = "nosniff";
        context.Response.Headers["Referrer-Policy"] = "no-referrer";
    }
}
