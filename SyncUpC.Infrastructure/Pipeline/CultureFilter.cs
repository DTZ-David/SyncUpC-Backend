using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Pipeline;


public class CultureFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var language = context.RouteData.Values["language"]?.ToString();
        SetCulture(language!);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private static void SetCulture(string language)
    {
        var cultureCode = language.ToLower() switch
        {
            "es" => "es-ES",
            _ => "en-US"
        };

        CultureInfo.CurrentCulture = new CultureInfo(cultureCode, false);
        CultureInfo.CurrentUICulture = new CultureInfo(cultureCode, false);
    }
}
