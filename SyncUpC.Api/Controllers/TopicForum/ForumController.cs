using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Commands.AddComment;
using SyncUpC.Application.UseCases.ForumUseCases.Commands.AddTopic;
using SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllCommentsForTopic;
using SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllTopicsForEvent;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.TopicForum;


/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class ForumController : BaseController
{
    [HttpPost]
    [Route("AddTopic")]
    [Authorize]
    public async Task<ActionResult<Response<Forum>>> AddTopic([FromBody] AddTopicCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("GetAllTopicsForEvent")]
    [Authorize]
    public async Task<ActionResult<Response<IEnumerable<Forum>>>> GetAllTopics([FromBody] GetAllTopicsForEventQuery command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("AddComment")]
    [Authorize]
    public async Task<ActionResult<Response<Forum>>> AddComment([FromBody] AddCommentCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPost]
    [Route("GetAllComments")]
    [Authorize]
    public async Task<ActionResult<Response<IEnumerable<Comment>>>> GetAllCommentsForTopic([FromBody] GetAllCommentsForTopicQuery command)
    {
        return await Mediator.Send(command);
    }



}
