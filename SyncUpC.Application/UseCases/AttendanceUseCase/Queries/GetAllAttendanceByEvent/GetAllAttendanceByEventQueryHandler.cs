using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
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

                userDtos.Add(new UserAttendanceDto(
                    user.Name,
                    user.LastName,
                    user.PhoneNumber,
                    userAttendance.CheckInTime
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
