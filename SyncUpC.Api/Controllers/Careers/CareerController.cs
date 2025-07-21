using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Careers.Queries.GetAllCareers;
using SyncUpC.Application.UseCases.Careers.Queries.GetCareersByFacultyId;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.CareerController;


/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class CareerController : BaseController
{
    /// <summary>
    /// Get all careers by facultiID.
    /// </summary>
    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>

    [HttpPost]
    [Route("GetAllCareersByFacultyId")]
    public async Task<ActionResult<Response<IEnumerable<Career>>>> GetAllCareersByFacultyId([FromBody] GetAllCareersByFacultyIdQuery command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet]
    [Route("GetAllCareers")]
    public async Task<ActionResult<Response<IEnumerable<Career>>>> GetAllCareers()
    {
        return await Mediator.Send(new GetAllCareersQuery());
    }
}
