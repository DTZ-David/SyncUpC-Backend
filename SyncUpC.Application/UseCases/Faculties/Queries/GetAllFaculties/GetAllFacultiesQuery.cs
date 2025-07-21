using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Faculties.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Factulties.Queries.GetAllFaculties;

public record GetAllFacultiesQuery : IRequest<ActionResult<Response<IEnumerable<FacultiesDto>>>>;
