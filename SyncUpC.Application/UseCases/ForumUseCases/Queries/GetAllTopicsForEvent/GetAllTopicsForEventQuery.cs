using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllTopicsForEvent;

public record GetAllTopicsForEventQuery(string eventId) : IRequest<ActionResult<Response<IEnumerable<Forum>>>>;
