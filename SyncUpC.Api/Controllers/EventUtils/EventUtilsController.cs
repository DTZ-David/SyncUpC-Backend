using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Campuses.Queries.GetAllCampus;
using SyncUpC.Application.UseCases.EventCategories.Queries.GetAllCategories;
using SyncUpC.Application.UseCases.EventTypes.Queries.GetAllEventTypes;
using SyncUpC.Application.UseCases.Spaces.Queries.GetAllSpaces;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.EventUtils;

/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class EventUtilsController : BaseController
{

    [Authorize]
    [HttpGet]
    [Route("GetAllSpaces")]
    public async Task<ActionResult<Response<IEnumerable<Space>>>> GetAllSpaces()
    {
        return await Mediator.Send(new GetAllSpacesQuery());
    }
    [Authorize]
    [HttpGet]
    [Route("GetAllCampus")]
    public async Task<ActionResult<Response<IEnumerable<Campus>>>> GetAllCampus()
    {
        return await Mediator.Send(new GetAllCampusQuery());
    }
    [Authorize]
    [HttpGet]
    [Route("GetAllEventCategories")]
    public async Task<ActionResult<Response<IEnumerable<EventCategory>>>> GetAllEventCategories()
    {
        return await Mediator.Send(new GetAllCategoriesQuery());
    }
    [Authorize]
    [HttpGet]
    [Route("GetAllEventTypes")]
    public async Task<ActionResult<Response<IEnumerable<EventType>>>> GetAllEventTypes()
    {
        return await Mediator.Send(new GetAllEventTypesQuery());
    }
}
