using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Queries.GetEventsForU;

public class GetEventsForUStudentQuery : IRequest<ActionResult<Response<IEnumerable<AcademicEventDto>>>>;
