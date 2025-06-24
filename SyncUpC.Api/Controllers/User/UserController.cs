using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.User.Students.Commands.CreateStudent;
using SyncUpC.Application.UseCases.User.Students.Commands.Login;
using SyncUpC.Application.UseCases.User.Students.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.User;


/// <summary>
/// Controller for managing areas related operations.
/// </summary>
[ApiController]
[Route(BaseRoute.BaseRouteUrl)]
public class UserController : BaseController
{

    /// <summary>
    /// Register a student user.
    /// </summary>
    /// <param name="language">The language for the response (e.g., "en", "es").</param>
    /// <response code="200">Successful query.</response>
    /// <response code="404">Query error, client's headquarters not found.</response>
    [HttpPost("registerStudent")]
    public async Task<ActionResult<Response<StudentDto>>> CreateUser([FromBody] CreateStudentCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Authenticate user in mobile apps
    /// </summary>
    /// <remarks>
    /// To authenticate in mobile apps it is necessary to provide the email and password
    /// </remarks>
    [HttpPost("loginApp")]
    public async Task<ActionResult<Response<AuthenticationUserDto>>> AuthenticationAppMovil([FromBody] AccountDto accountDto)
    {
        var command = new AuthenticationUserCommand(accountDto.Email, accountDto.Password);
        return await Mediator.Send(command);
    }
}
