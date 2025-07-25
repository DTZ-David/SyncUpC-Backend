using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllCommentsForTopic;

public record GetAllCommentsForTopicQuery(string forumId) : IRequest<ActionResult<Response<IEnumerable<Comment>>>>;
