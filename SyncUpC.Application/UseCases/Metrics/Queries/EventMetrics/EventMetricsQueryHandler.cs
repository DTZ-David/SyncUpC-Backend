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

namespace SyncUpC.Application.UseCases.Metrics.Queries.EventMetrics;

public class EventMetricsQueryHandler : IRequestHandler<EventMetricsQuery, ActionResult<Response<EventMetricsDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EventMetricsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ActionResult<Response<EventMetricsDto>>> Handle(EventMetricsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Obtener eventos filtrados
            var events = await _unitOfWork.EventService.GetEventsFilteredAsync(
                request.DateFrom, request.DateTo, request.Faculty, request.Program, request.EventType, request.Category
            ) ?? new List<AcademicEvent>();

            // 2. Total de eventos
            var totalEvents = events.Count;

            // 3. Variación de eventos (% comparado con periodo anterior)
            double percentageChangeEvents = 0;
            if (request.DateFrom.HasValue && request.DateTo.HasValue)
            {
                var previousFrom = request.DateFrom.Value.AddDays(-(request.DateTo.Value - request.DateFrom.Value).TotalDays);
                var previousTo = request.DateFrom.Value.AddDays(-1);

                var previousEvents = await _unitOfWork.EventService.GetEventsFilteredAsync(
                    previousFrom, previousTo, request.Faculty, request.Program, request.EventType, request.Category
                ) ?? new List<AcademicEvent>();

                var prevCount = previousEvents.Count;
                if (prevCount > 0)
                    percentageChangeEvents = Math.Round(((double)(totalEvents - prevCount) / prevCount) * 100, 1);
            }

            // 4. Obtener registros y asistencias
            var registrations = new List<Registration>();
            var attendances = new List<Attendance>();

            foreach (var eventId in events.Select(e => e.Id))
            {
                var reg = await _unitOfWork.RegistrationService.GetRegistrationOfEvent(eventId);
                if (reg != null)
                    registrations.Add(reg);

                var att = await _unitOfWork.AttendanceService.GetAttendance(eventId);
                if (att != null)
                    attendances.Add(att);
            }

            var totalRegistrations = registrations.SelectMany(r => r.RegistratedUsers).Count();
            var totalAttendances = attendances.SelectMany(a => a.UserAttendances).Count();

            // 5. Promedio de tasa de asistencia (asistencias / registros)
            double averageAttendanceRate = totalRegistrations > 0
                ? Math.Round((double)totalAttendances / totalRegistrations * 100, 1)
                : 0;


            // 6. Variación de asistencia (% comparado con periodo anterior)
            double percentageChangeAttendance = 0;
            if (request.DateFrom.HasValue && request.DateTo.HasValue)
            {
                var previousFrom = request.DateFrom.Value.AddDays(-(request.DateTo.Value - request.DateFrom.Value).TotalDays);
                var previousTo = request.DateFrom.Value.AddDays(-1);

                var previousEvents = await _unitOfWork.EventService.GetEventsFilteredAsync(
                    previousFrom, previousTo, request.Faculty, request.Program, request.EventType, request.Category
                ) ?? new List<AcademicEvent>();

                var prevEventIds = previousEvents.Select(e => e.Id).ToList();
                var prevAttendances = new List<Attendance>();
                foreach (var eventId in prevEventIds)
                {
                    var atts = await _unitOfWork.AttendanceService.GetAttendance(eventId);
                    prevAttendances.AddRange(atts);
                }

                var prevAttendancesCount = prevAttendances.SelectMany(a => a.UserAttendances).Count();
                if (prevAttendancesCount > 0)
                    percentageChangeAttendance = Math.Round(((double)(totalAttendances - prevAttendancesCount) / prevAttendancesCount) * 100, 1);
            }

            // 7. Índice de cumplimiento (% eventos con >= 70% de asistencia respecto a registros)
            double complianceIndex = 0;
            if (events.Any())
            {
                var compliantEvents = events.Count(e =>
                {
                    var evRegs = registrations.Where(r => r.EventId == e.Id).SelectMany(r => r.RegistratedUsers).Count();
                    var evAtts = attendances.Where(a => a.EventId == e.Id).SelectMany(a => a.UserAttendances).Count();

                    return evRegs > 0 && (double)evAtts / evRegs >= 0.7;
                });

                complianceIndex = Math.Round((double)compliantEvents / events.Count * 100, 1);
            }

            // 8. Promedio de ocupación (asistencias / registros por evento)
            double averageOccupancy = events.Any()
                ? Math.Round(events.Average(e =>
                {
                    var evRegs = registrations.Where(r => r.EventId == e.Id).SelectMany(r => r.RegistratedUsers).Count();
                    var evAtts = attendances.Where(a => a.EventId == e.Id).SelectMany(a => a.UserAttendances).Count();
                    return evRegs > 0 ? (double)evAtts / evRegs * 100 : 0;
                }), 1)
                : 0;

            // 9. Top 3 eventos
            var topEvents = events.Select(e =>
            {
                var evRegs = registrations.Where(r => r.EventId == e.Id).SelectMany(r => r.RegistratedUsers).Count();
                var evAtts = attendances.Where(a => a.EventId == e.Id).SelectMany(a => a.UserAttendances).Count();

                var occupancy = evRegs > 0 ? Math.Round((double)evAtts / evRegs * 100, 1) : 0;

                return new TopEventDto(e.EventTitle, evAtts, occupancy);
            })
            .OrderByDescending(te => te.Attendees)
            .Take(3)
            .ToList();

            // 10. Evolución mensual
            var monthlyEvolution = events
                .GroupBy(e => $"{e.StartDate:yyyy-MM}")
                .Select(g =>
                {
                    var monthEvents = g.ToList();
                    var monthEventIds = monthEvents.Select(ev => ev.Id).ToList();

                    var monthAtts = attendances.Where(a => monthEventIds.Contains(a.EventId))
                        .SelectMany(a => a.UserAttendances)
                        .Count();

                    return new MonthlyParticipationDto(
                        g.Key,
                        monthEvents.Count,
                        monthAtts
                    );
                })
                .OrderBy(m => m.Month)
                .ToList();

            var resultDto = new EventMetricsDto(
                totalEvents,
                percentageChangeEvents,
                averageAttendanceRate,
                percentageChangeAttendance,
                complianceIndex,
                averageOccupancy,
                topEvents,
                monthlyEvolution
            );

            return new OkObjectResult(new Response<EventMetricsDto>((int)MessageStatusCode.Success, resultDto));
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message, (int)MessageStatusCode.BadRequest);
        }
    }


}
