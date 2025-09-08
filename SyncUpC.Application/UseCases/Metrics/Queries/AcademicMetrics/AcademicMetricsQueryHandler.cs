using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Metrics.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Attendance;
using SyncUpC.Domain.Entities.Registration;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Metrics.Queries.AcademicMetrics
{
    public class AcademicMetricsQueryHandler : IRequestHandler<AcademicMetricsQuery, ActionResult<Response<AcademicMetricsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AcademicMetricsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<AcademicMetricsDto>>> Handle(AcademicMetricsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // ✅ Asegurar que ninguna colección sea nula
                var allEvents = await _unitOfWork.EventService.GetAllEventsForMetrics();

                var filteredEvents = allEvents.AsQueryable();

                if (request.DateFrom.HasValue)
                    filteredEvents = filteredEvents.Where(e => e.StartDate >= request.DateFrom.Value);
                if (request.DateTo.HasValue)
                    filteredEvents = filteredEvents.Where(e => e.StartDate <= request.DateTo.Value);
                if (!string.IsNullOrEmpty(request.EventType))
                    filteredEvents = filteredEvents.Where(e => e.EventTypes != null && e.EventTypes.Any(et => et.Name == request.EventType));
                if (!string.IsNullOrEmpty(request.Category))
                    filteredEvents = filteredEvents.Where(e => e.Categories != null && e.Categories.Any(et => et.Name == request.Category));

                var eventsList = filteredEvents.ToList();
                var eventIds = eventsList.Select(e => e.Id).ToList();

                // ✅ Inscripciones
                var allRegistrations = await _unitOfWork.RegistrationService.GetAllRegistration() ?? new List<Registration>();
                var filteredRegistrations = allRegistrations.Where(r => r != null && eventIds.Contains(r.EventId)).ToList();

                // ✅ Asistencias
                var allAttendances = new List<Attendance>();
                foreach (var eventId in eventIds)
                {
                    var attendances = await _unitOfWork.AttendanceService.GetAttendance(eventId);
                    allAttendances.AddRange(attendances);
                }
                var filteredAttendances = allAttendances.Where(a => a != null && eventIds.Contains(a.EventId)).ToList();

                // ✅ Usuarios y facultades
                var userIds = filteredRegistrations
                    .SelectMany(r => r.RegistratedUsers ?? new List<UserRegistration>())
                    .Where(u => u != null)
                    .Select(u => u.UserId)
                    .Distinct()
                    .ToList();

                var userFaculties = await _unitOfWork.UserService.GetUserFaculties(userIds) ?? new Dictionary<string, string>();

                var totalUsers = userIds.Count;

                var facultyDistribution = filteredRegistrations
                    .SelectMany(r => r.RegistratedUsers ?? new List<UserRegistration>())
                    .Where(u => u != null)
                    .GroupBy(u => userFaculties.ContainsKey(u.UserId) ? userFaculties[u.UserId] : "Desconocida")
                    .Select(g => new FacultyDistributionDto(
                        g.Key,
                        g.Count(),
                        totalUsers > 0 ? Math.Round((double)g.Count() / totalUsers * 100, 1) : 0
                    ))
                    .ToList();

                var eventTypeAttendance = eventsList
                    .GroupBy(e => e.EventTypes?.FirstOrDefault()?.Name ?? "Sin tipo")
                    .Select(g =>
                    {
                        var ids = g.Select(ev => ev.Id).ToList();
                        var totalEvents = g.Count();
                        var totalAttendances = filteredAttendances.Count(a => ids.Contains(a.EventId));

                        var ratio = totalEvents > 0
                            ? (int)Math.Round(totalAttendances / (double)totalEvents)
                            : 0;

                        return new EventTypeAttendanceDto(g.Key, totalEvents, ratio);
                    })
                    .ToList();

                var timeSlotTrends = eventsList
                    .GroupBy(e => GetTimeSlot(e.StartDate))
                    .Select(g => new TimeSlotTrendDto(
                        g.Key,
                        g.Count(),
                        g.Any() ? (int)Math.Round(filteredAttendances.Count(a => g.Select(ev => ev.Id).Contains(a.EventId)) / (double)g.Count()) : 0
                    ))
                    .ToList();

                var weeklyParticipation = eventsList
                    .GroupBy(e => e.StartDate.DayOfWeek.ToString())
                    .Select(g => new WeeklyParticipationDto(
                        g.Key,
                        g.Any() ? (int)Math.Round(filteredAttendances.Count(a => g.Select(ev => ev.Id).Contains(a.EventId)) / (double)g.Count()) : 0,
                        g.Count()
                    ))
                    .ToList();

                var resultDto = new AcademicMetricsDto(
                    facultyDistribution,
                    eventTypeAttendance,
                    timeSlotTrends,
                    weeklyParticipation
                );

                return new OkObjectResult(new Response<AcademicMetricsDto>((int)MessageStatusCode.Success, resultDto));
            }
            catch (Exception ex)
            {
                // 🔥 Capturas cualquier error y lo devuelves en la respuesta
                throw new BusinessException(ex.Message, (int)MessageStatusCode.BadRequest);
            }
        }


        private string GetTimeSlot(DateTime startTime)
        {
            var hour = startTime.Hour;
            return hour switch
            {
                >= 8 and < 12 => "08:00-12:00",
                >= 14 and < 18 => "14:00-18:00",
                >= 18 and < 20 => "18:00-20:00",
                _ => "Otro"
            };
        }
    }
}
