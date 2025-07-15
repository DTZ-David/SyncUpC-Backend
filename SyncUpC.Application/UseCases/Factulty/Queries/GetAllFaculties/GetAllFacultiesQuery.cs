using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Application.UseCases.Factulty.Queries.GetAllFaculties;

public record GetAllFacultiesQuery : IRequest<ActionResult<Response<IEnumerable<Faculty>>>>;
