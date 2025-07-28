using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddComment;

public record AddCommentCommand(
    string forumId,
    string content
    ) : IRequest<ActionResult<Response<ForumDto>>>;

