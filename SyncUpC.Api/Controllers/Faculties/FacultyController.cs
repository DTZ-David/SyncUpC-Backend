using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Factulties.Queries.GetAllFaculties;
using SyncUpC.Application.UseCases.Faculties.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.Faculties;

/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class FacultyController : BaseController
{


    [HttpGet]
    [Route("GetAllFaculties")]
    public async Task<ActionResult<Response<IEnumerable<FacultiesDto>>>> GetAllFaculties()
    {
        return await Mediator.Send(new GetAllFacultiesQuery());
    }
}
