using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Commands.CreateEvent;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.Event;

/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class EventController : BaseController
{
    /// <summary>
    /// Create a event.
    /// </summary>

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("CreateEvent")]
    public async Task<ActionResult<Response<AcademicEventDto>>> CreateUser([FromBody] CreateEventCommand command)
    {
        return await Mediator.Send(command);
    }
}
