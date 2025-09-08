using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Metrics.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Attendance;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Entities.Registration;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Metrics.Queries.UserMetrics;

public class UserMetricsQueryHandler : IRequestHandler<UserMetricsQuery, ActionResult<Response<UserMetricsDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserMetricsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<UserMetricsDto>>> Handle(UserMetricsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Obtener eventos filtrados
            var events = await _unitOfWork.EventService.GetEventsFilteredAsync(
                request.DateFrom, request.DateTo, request.Faculty, request.Program, request.EventType, request.Category
            ) ?? new List<AcademicEvent>();

            // Registrations
            var registrations = new List<Registration>();
            foreach (var eventId in events.Select(e => e.Id))
            {
                var reg = await _unitOfWork.RegistrationService.GetRegistrationOfEvent(eventId);
                if (reg != null)
                    registrations.Add(reg);
            }

            // Attendances
            var attendances = new List<Attendance>();
            foreach (var eventId in events.Select(e => e.Id))
            {
                var att = await _unitOfWork.AttendanceService.GetAttendance(eventId);
                if (att != null)
                    attendances.Add(att);
            }

            // Calcular usuarios únicos
            var uniqueUsers = registrations
                .SelectMany(r => r.RegistratedUsers ?? new List<UserRegistration>())
                .Select(u => u.UserId)
                .Distinct()
                .ToList();

            var activeUsers = uniqueUsers.Count;

            // Usuarios recurrentes
            var userEventCounts = registrations
                .SelectMany(r => r.RegistratedUsers ?? new List<UserRegistration>())
                .GroupBy(u => u.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            var recurrentUsers = userEventCounts.Count(u => u.Value > 1);

            // Tasa de retención
            var userRetentionRate = activeUsers > 0 ? Math.Round((double)recurrentUsers / activeUsers * 100, 1) : 0;

            // Promedio de participación
            var averageParticipation = activeUsers > 0 ? Math.Round((double)registrations.Count / activeUsers, 1) : 0;

            // Top usuarios
            var topUsers = userEventCounts
                .OrderByDescending(u => u.Value)
                .Take(3)
                .Select(u =>
                {
                    var userRegistration = registrations
                        .SelectMany(r => r.RegistratedUsers ?? new List<UserRegistration>())
                        .FirstOrDefault(ur => ur.UserId == u.Key);

                    var userName = userRegistration?.UserId ?? "Desconocido";

                    var lastAttendance = attendances
                        .SelectMany(a => a.UserAttendances ?? new List<UserAttendance>())
                        .Where(ua => ua.UserId == u.Key)
                        .OrderByDescending(ua => DateTime.TryParse(ua.CheckInTime, out var dt) ? dt : DateTime.MinValue)
                        .FirstOrDefault();

                    return new TopUserDto(
                        userName,
                        lastAttendance != null && DateTime.TryParse(lastAttendance.CheckInTime, out var checkInDate)
                            ? checkInDate.ToString("yyyy-MM-dd")
                            : "Sin asistencia",
                        u.Value
                    );
                })
                .ToList();

            // Nuevos vs recurrentes por evento
            var newVsRecurrentByEvent = events.Take(3).Select(e =>
            {
                var eventRegistrations = registrations.Where(r => r.EventId == e.Id).ToList();

                var eventUserIds = eventRegistrations
                    .SelectMany(r => r.RegistratedUsers ?? new List<UserRegistration>())
                    .Select(ur => ur.UserId)
                    .ToList();

                var newUsers = eventUserIds.Count(userId =>
                    !registrations.Any(r => r.EventId != e.Id &&
                                            (r.RegistratedUsers ?? new List<UserRegistration>())
                                            .Any(ur => ur.UserId == userId)));

                var recurrentUsersForEvent = eventUserIds.Count - newUsers;

                return new NewVsRecurrentDto(
                    e.EventTitle,
                    newUsers,
                    recurrentUsersForEvent
                );
            }).ToList();

            var resultDto = new UserMetricsDto(
                userRetentionRate,
                activeUsers,
                averageParticipation,
                recurrentUsers,
                topUsers,
                newVsRecurrentByEvent
            );

            return new OkObjectResult(new Response<UserMetricsDto>((int)MessageStatusCode.Success, resultDto));
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message, (int)MessageStatusCode.BadRequest);
        }
    }

}
