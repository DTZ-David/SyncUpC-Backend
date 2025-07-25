using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Bookmarks.Commands.RemoveSaveEvent;
using SyncUpC.Application.UseCases.Bookmarks.Commands.SavedEvents;
using SyncUpC.Application.UseCases.Bookmarks.Queries.GetAllSavedEvents;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.Bookmarks;

/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class BookmarkController : BaseController
{

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("AddEventoToFavs")]
    public async Task<ActionResult<Response<AcademicEventDto>>> SavedEvents([FromBody] SavedEventCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("RemoveSaveEvent")]
    public async Task<ActionResult<Response<AcademicEventDto>>> RemoveSaveEvent([FromBody] RemoveSaveEventCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpGet]
    [Route("GetAllSavedEvents")]
    public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> GetAllSavedEvents()
    {
        return await Mediator.Send(new GetAllSavedEventsQuery());
    }
}
