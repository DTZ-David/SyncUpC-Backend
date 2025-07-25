using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Bookmarks.Queries.GetAllSavedEvents;

public record GetAllSavedEventsQuery : IRequest<ActionResult<Response<IEnumerable<AcademicEventDto>>>>;
