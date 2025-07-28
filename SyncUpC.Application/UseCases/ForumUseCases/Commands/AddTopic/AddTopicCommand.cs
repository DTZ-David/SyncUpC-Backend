using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddTopic
{
    public record AddTopicCommand(

        string eventId,
        string title,
        string content
        ) : IRequest<ActionResult<Response<ForumDto>>>;

}
