using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Careers.Queries.GetAllCareers;

public record GetAllCareersQuery : IRequest<ActionResult<Response<IEnumerable<Domain.Entities.User.Career>>>>;
