using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddComment;

public record AddCommentCommand(
    string forumId,
    string content
    ) : IRequest<ActionResult<Response<Forum>>>;

