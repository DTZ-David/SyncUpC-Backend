using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetForumById;

public record GetForumByIdQuery(string forumId) : IRequest<ActionResult<Response<ForumDto>>>;
