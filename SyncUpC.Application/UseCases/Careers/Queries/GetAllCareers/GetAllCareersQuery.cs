using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Application.UseCases.Careers.Queries.GetAllCareers;

public record GetAllCareersQuery : IRequest<ActionResult<Response<IEnumerable<Career>>>>;
