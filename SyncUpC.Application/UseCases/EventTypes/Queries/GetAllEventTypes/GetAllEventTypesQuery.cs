using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Application.UseCases.EventTypes.Queries.GetAllEventTypes;

public record GetAllEventTypesQuery : IRequest<ActionResult<Response<IEnumerable<EventType>>>>;


