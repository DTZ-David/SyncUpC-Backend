using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Attendance;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.AttendanceUseCase.Commands.FillAttendance;

public class CheckInAttendanceCommandHandler : IRequestHandler<CheckInAttendanceCommand, ActionResult<Response<AttendanceDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CheckInAttendanceCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<AttendanceDto>>> Handle(CheckInAttendanceCommand request, CancellationToken cancellationToken)
    {
        // Obtener usuario autenticado
        var claims = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(claims.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Crear el objeto UserAttendance
        var userAttendance = new UserAttendance(
            userId: user.Id,
            checkInTime: DateTime.Now.ToString(),
            checkOutTime: ""
        );

        // Registrar asistencia (crear o actualizar)
        var attendance = await _unitOfWork.AttendanceService.SubmitAnAttendance(userAttendance, request.eventId);

        var events = await _unitOfWork.EventService.GetEventById(attendance.EventId);

        var attendanceDto = new AttendanceDto(events.Id, events.EventTitle);

        return new CreatedResult(string.Empty, new Response<AttendanceDto>((int)MessageStatusCode.Create, attendanceDto));
    }
}
