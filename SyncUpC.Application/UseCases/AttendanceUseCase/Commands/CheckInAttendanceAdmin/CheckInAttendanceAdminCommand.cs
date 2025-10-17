using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Application.UseCases.AttendanceUseCase.Commands.CheckInAttendanceAdmin;

public record CheckInAttendanceAdminCommand(string eventId, string userId) : IRequest<ActionResult<Response<AttendanceDto>>>;
