using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Application.UseCases.Spaces.Queries.GetAllSpaces;

public record GetAllSpacesQuery : IRequest<ActionResult<Response<IEnumerable<Space>>>>;
