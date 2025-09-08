using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Metrics.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Metrics.Queries.UserMetrics;

public record UserMetricsQuery(
    DateTime? DateFrom,
    DateTime? DateTo,
    string? Faculty,
    string? Program,
    string? EventType,
    string? Category
) : IRequest<ActionResult<Response<UserMetricsDto>>>;
