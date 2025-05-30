using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SyncUpC.WebApi.Controllers;

[Produces("application/json")]
[ProducesResponseType(200)]
[ProducesResponseType(400)]
[ProducesResponseType(401)]
[ProducesResponseType(404)]
[ProducesResponseType(500)]

public class BaseController : ControllerBase
{
    /// <summary>
    /// Mediator instance for sending requests to handlers.
    /// </summary>
    protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>()!;
}
