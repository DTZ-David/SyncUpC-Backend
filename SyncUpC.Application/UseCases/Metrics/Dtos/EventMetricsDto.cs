namespace SyncUpC.Application.UseCases.Metrics.Dtos;

public record EventMetricsDto(
  int TotalEvents,
  double PercentageChangeEvents,
  double AverageAttendanceRate,
  double PercentageChangeAttendance,
  double ComplianceIndex,
  double AverageOccupancy,
  List<TopEventDto> TopEvents,
  List<MonthlyParticipationDto> MonthlyEvolution
);

public record TopEventDto(
    string EventName,
    int Attendees,
    double OccupancyPercentage
);

public record MonthlyParticipationDto(
    string Month,
    int Events,
    int Attendees
);
