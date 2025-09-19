using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.AttendanceUseCase.Queries.GetAllAttendanceByEvent
{
    public class GetAllAttendanceByEventQueryHandler : IRequestHandler<GetAllAttendanceByEventQuery, ActionResult<Response<GetAttendanceRecordDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAttendanceByEventQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<GetAttendanceRecordDto>>> Handle(GetAllAttendanceByEventQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _unitOfWork.AttendanceService.GetAttendance(request.eventId);
            var userDtos = new List<UserAttendanceDto>();

            foreach (var userAttendance in attendance.UserAttendances)
            {
                var user = await _unitOfWork.UserService.GetUserById(userAttendance.UserId);

                // Variables para Career y Faculty
                string careerName = null;
                string facultyName = null;

                // Verificar si el usuario es un Student
                if (user is Student student)
                {
                    careerName = student.Career?.Name; // Asumiendo que Career tiene una propiedad Name
                    facultyName = student.Faculty?.Name; // Asumiendo que Faculty tiene una propiedad Name
                }

                userDtos.Add(new UserAttendanceDto(
                    user.Name,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber,
                    userAttendance.CheckInTime,
                    careerName,  // Agregar career
                    facultyName  // Agregar faculty
                ));
            }

            var recordDto = new GetAttendanceRecordDto(
                attendance.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                attendance.EventId,
                userDtos
            );

            return new OkObjectResult(
                new Response<GetAttendanceRecordDto>(
                    (int)MessageStatusCode.Success,
                    recordDto
                )
            );
        }
    }
}