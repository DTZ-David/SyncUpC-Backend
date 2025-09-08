namespace SyncUpC.Application.UseCases.Metrics.Dtos;

public record AcademicMetricsDto(
 List<FacultyDistributionDto> FacultyDistribution,
 List<EventTypeAttendanceDto> AttendanceByEventType,
 List<TimeSlotTrendDto> TimeSlotTrends,
 List<WeeklyParticipationDto> WeeklyParticipation
);

public record FacultyDistributionDto(
    string FacultyName,
    int Students,
    double Percentage
);

public record EventTypeAttendanceDto(
    string EventType,
    int TotalEvents,
    int AverageAttendance
);

public record TimeSlotTrendDto(
    string TimeSlot,
    int Events,
    int AverageAttendance
);

public record WeeklyParticipationDto(
    string DayOfWeek,
    int AverageAttendance,
    int TotalEvents
);
