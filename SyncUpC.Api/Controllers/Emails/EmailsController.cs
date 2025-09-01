using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Emails.Commands.SendEmailsForEvent;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.Emails
{

    /// <summary>
    /// Controller for managing areas related operations.
    /// </summary>
    [ApiController]
    [Route(BaseRoute.BaseRouteUrl)]
    public class EmailsController : BaseController
    {
        /// <summary>
        /// Get all careers by facultiID.
        /// </summary>
        /// <response code="200">Successful query.</response>
        /// <response code="404">Query error, client's headquarters not found.</response>

        [HttpPost]
        [Authorize]
        [Route("SendEmails")]
        public async Task<ActionResult<Response<AcademicEventDto>>> SendEmails([FromBody] SendEmailsForEventCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
