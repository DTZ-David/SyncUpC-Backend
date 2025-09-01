using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Registration;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.RegistrationUseCases.Commands.RegisterEvent;

public class RegisterEventCommandHandler : IRequestHandler<RegisterEventCommand, ActionResult<Response<Registration>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<Registration>>> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
    {
        var claims = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(claims.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Crear el objeto UserAttendance
        var userRegistration = new UserRegistration(
            userId: user.Id,
            registrationDate: DateTime.Now
        );

        // Registrar asistencia (crear o actualizar)
        var attendance = await _unitOfWork.RegistrationService.RegistrationEvent(userRegistration, request.eventId);

        var events = await _unitOfWork.EventService.GetEventById(attendance.EventId);

        var attendanceDto = new AttendanceDto(events.Id, events.EventTitle);

        return new CreatedResult(string.Empty, new Response<AttendanceDto>((int)MessageStatusCode.Create, attendanceDto));
    }
}
