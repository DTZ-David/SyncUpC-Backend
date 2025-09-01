using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.AttendanceUseCase.Queries.GetAllAttendanceByEvent;

public record GetAllAttendanceByEventQuery(string eventId) : IRequest<ActionResult<Response<GetAttendanceRecordDto>>>;

