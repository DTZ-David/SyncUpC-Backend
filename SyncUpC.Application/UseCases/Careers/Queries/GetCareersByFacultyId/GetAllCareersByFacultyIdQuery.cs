using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Application.UseCases.Careers.Queries.GetCareersByFacultyId;

public record GetAllCareersByFacultyIdQuery(string facultyId) : IRequest<ActionResult<Response<IEnumerable<Career>>>>;

