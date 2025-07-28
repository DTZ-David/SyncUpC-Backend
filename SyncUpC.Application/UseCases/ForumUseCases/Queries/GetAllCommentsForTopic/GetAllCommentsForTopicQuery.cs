using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllCommentsForTopic;

public record GetAllCommentsForTopicQuery(string forumId) : IRequest<ActionResult<Response<IEnumerable<CommentDto>>>>;
