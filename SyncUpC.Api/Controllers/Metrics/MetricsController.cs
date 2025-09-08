using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Metrics.Dtos;
using SyncUpC.Application.UseCases.Metrics.Queries.AcademicMetrics;
using SyncUpC.Application.UseCases.Metrics.Queries.EventMetrics;
using SyncUpC.Application.UseCases.Metrics.Queries.UserMetrics;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.WebApi.Common.Constants;

namespace SyncUpC.WebApi.Controllers.Metrics
{
    /// <summary>
    /// Controller for managing areas related operations.
    /// </summary>
    [ApiController]
    [Route(BaseRoute.BaseRouteUrl)]
    public class MetricsController : BaseController
    {
        [HttpPost]
        [Route("GetAcademicMetrics")]
        [Authorize]
        public async Task<ActionResult<Response<AcademicMetricsDto>>> GetAcademicMetrics([FromBody] AcademicMetricsQuery command)
        {
            return await Mediator.Send(command);
        }


        [HttpPost]
        [Route("GetUserMetrics")]
        [Authorize]
        public async Task<ActionResult<Response<UserMetricsDto>>> GetUserMetrics([FromBody] UserMetricsQuery command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("GetEventMetrics")]
        [Authorize]
        public async Task<ActionResult<Response<EventMetricsDto>>> GetEventMetrics([FromBody] EventMetricsQuery command)
        {
            return await Mediator.Send(command);
        }
    }
}


