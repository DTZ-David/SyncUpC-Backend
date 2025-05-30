using Microsoft.AspNetCore.Mvc;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.User;


/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class UserController : BaseController
{

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Prueba exitosa");
    }
}
