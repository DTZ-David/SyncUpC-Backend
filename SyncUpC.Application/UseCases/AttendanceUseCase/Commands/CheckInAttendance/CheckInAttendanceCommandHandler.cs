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

        // Consultar el evento
        var eventEntity = await _unitOfWork.EventService.GetEventById(request.eventId)
            ?? throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

        var now = DateTime.UtcNow;
        if (eventEntity.EndDate < now)
        {
            throw new BusinessException("El evento ya finalizó, no es posible registrar asistencia.", (int)MessageStatusCode.BadRequest);
        }

        if (eventEntity.StartDate > now)
        {
            throw new BusinessException("El evento aún no ha iniciado, no es posible registrar asistencia.", (int)MessageStatusCode.BadRequest);
        }

        // Crear el objeto UserAttendance
        var userAttendance = new UserAttendance(
            userId: user.Id,
            checkInTime: now.ToString("o")
        );


        var attendance = await _unitOfWork.AttendanceService.SubmitAnAttendance(userAttendance, request.eventId);

        var attendanceDto = new AttendanceDto(eventEntity.Id, eventEntity.EventTitle);

        return new CreatedResult(string.Empty, new Response<AttendanceDto>((int)MessageStatusCode.Create, attendanceDto));
    }

}
