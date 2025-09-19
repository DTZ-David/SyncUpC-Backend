using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Commands.CreateEvent;
using SyncUpC.Application.UseCases.Events.Commands.DeleteEvent;
using SyncUpC.Application.UseCases.Events.Commands.UpdateEvent;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Application.UseCases.Events.Queries.GetAllEvents;
using SyncUpC.Application.UseCases.Events.Queries.GetAllEventsForStaffMember;
using SyncUpC.Application.UseCases.Events.Queries.GetEventsForU;
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

    [Authorize]
    [HttpPost]
    [Route("UpdateEvent")]
    public async Task<ActionResult<Response<AcademicEventDto>>> UpdateEvent([FromBody] UpdateEventCommand command)
    {
        return await Mediator.Send(command);
    }
    [Authorize]
    [HttpDelete]
    [Route("DeleteEvent")]
    public async Task<ActionResult<Response<AcademicEventDto>>> DeleteEvent([FromBody] DeleteEventCommand command)
    {
        return await Mediator.Send(command);
    }

    [Authorize]
    [HttpGet]
    [Route("GetAllEvents")]
    public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> GetAllEvents()
    {
        return await Mediator.Send(new GetAllEventsQuery());
    }

    [Authorize]
    [HttpGet]
    [Route("GetAllEventsForStudents")]
    public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> GetAllEventsForStudents()
    {
        return await Mediator.Send(new GetEventsForUStudentQuery());
    }

    [Authorize]
    [HttpGet]
    [Route("GetAllEventsMadeForU")]
    public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> GetAllEventsMadeForU()
    {
        return await Mediator.Send(new GetAllEventsForStaffMemberQuery());
    }
}
