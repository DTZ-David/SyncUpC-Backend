using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Commands.AddImages;
using SyncUpC.Application.UseCases.Events.Commands.DeleteEventImages;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Application.UseCases.Events.Queries.GetEventImagesById;
using SyncUpC.Application.UseCases.Events.Queries.GetUserEventImages;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.Event;

/// <summary>
/// Controller for managing event images operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class EventImagesController : BaseController
{
    /// <summary>
    /// Add images to an event.
    /// </summary>
    /// <response code="200">Successful operation.</response>
    /// <response code="404">Event not found.</response>
    [Authorize]
    [HttpPost]
    [Route("AddEventImages")]
    public async Task<ActionResult<Response<EventImagesDto>>> AddEventImages([FromBody] AddImagesCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Get all images for a specific event.
    /// </summary>
    /// <response code="200">Successful query.</response>
    /// <response code="404">Event not found.</response>
    [Authorize]
    [HttpPost]
    [Route("GetEventImages")]
    public async Task<ActionResult<Response<List<EventImagesDto>>>> GetEventImages([FromBody] GetEventImagesByIdQuery query)
    {
        return await Mediator.Send(query);
    }


    /// <summary>
    /// Get all images uploaded by current user.
    /// </summary>
    /// <response code="200">Successful query.</response>
    [Authorize]
    [HttpGet]
    [Route("GetMyEventImages")]
    public async Task<ActionResult<Response<List<EventImagesDto>>>> GetMyEventImages()
    {
        return await Mediator.Send(new GetUserEventImagesQuery());
    }

    /// <summary>
    /// Delete event images.
    /// </summary>
    /// <response code="200">Successful operation.</response>
    /// <response code="404">Images not found.</response>
    [Authorize]
    [HttpDelete]
    [Route("DeleteEventImages")]
    public async Task<ActionResult<Response<bool>>> DeleteEventImages([FromBody] DeleteEventImagesCommand command)
    {
        return await Mediator.Send(command);
    }
}