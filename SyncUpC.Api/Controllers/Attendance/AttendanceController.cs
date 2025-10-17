using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Commands.CheckInAttendanceAdmin;
using SyncUpC.Application.UseCases.AttendanceUseCase.Commands.FillAttendance;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Application.UseCases.AttendanceUseCase.Queries.GetAllAttendanceByEvent;
using SyncUpC.Application.UseCases.RegistrationUseCases.Commands.RegisterEvent;
using SyncUpC.Application.UseCases.RegistrationUseCases.Commands.UnregisterEvent;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Registration;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.AttendanceController;

/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class AttendanceController : BaseController
{
    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("CheckIn")]
    public async Task<ActionResult<Response<AttendanceDto>>> SavedEvents([FromBody] CheckInAttendanceCommand command)
    {
        return await Mediator.Send(command);
    }
    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("CheckInAdmin")]
    public async Task<ActionResult<Response<AttendanceDto>>> CheckInAdmin([FromBody] CheckInAttendanceAdminCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("RegisterEvent")]
    public async Task<ActionResult<Response<Registration>>> RegisterEvent([FromBody] RegisterEventCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [Authorize]
    [HttpPost]
    [Route("DeleteRegistration")]
    public async Task<ActionResult<Response<Registration>>> DeleteRegistration([FromBody] UnregisterEventCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [HttpPost]
    [Route("AttendanceList")]
    public async Task<ActionResult<Response<GetAttendanceRecordDto>>> GetAttendance([FromBody] GetAllAttendanceByEventQuery command)
    {
        return await Mediator.Send(command);
    }
}
