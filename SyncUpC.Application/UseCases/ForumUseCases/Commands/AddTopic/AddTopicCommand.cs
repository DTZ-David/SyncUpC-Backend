using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddTopic
{
    public record AddTopicCommand(

        string eventId,
        string title,
        string content
        ) : IRequest<ActionResult<Response<Forum>>>;

}
