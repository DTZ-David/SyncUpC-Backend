using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllTopicsForEvent;

public record GetAllTopicsForEventQuery(string eventId) : IRequest<ActionResult<Response<IEnumerable<ForumDto>>>>;
