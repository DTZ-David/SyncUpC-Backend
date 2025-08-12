using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.AttendanceUseCase.Commands.FillAttendance;

public record CheckInAttendanceCommand(string eventId) : IRequest<ActionResult<Response<AttendanceDto>>>;
