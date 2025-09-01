using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Application.UseCases.Campuses.Queries.GetAllCampus;

public record GetAllCampusQuery : IRequest<ActionResult<Response<IEnumerable<Campus>>>>;

