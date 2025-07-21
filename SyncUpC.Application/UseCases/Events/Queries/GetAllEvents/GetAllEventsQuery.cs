using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Queries.GetAllEvents;

public class GetAllEventsQuery : IRequest<ActionResult<Response<IEnumerable<AcademicEventDto>>>>;
