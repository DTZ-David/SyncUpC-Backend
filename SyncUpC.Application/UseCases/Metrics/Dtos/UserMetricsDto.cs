namespace SyncUpC.Application.UseCases.Metrics.Dtos;

public record UserMetricsDto(
  double UserRetentionRate,
  int ActiveUsers,
  double AverageParticipation,
  int RecurrentUsers,
  List<TopUserDto> TopUsers,
  List<NewVsRecurrentDto> NewVsRecurrentByEvent
);

public record TopUserDto(
    string UserName,
    string LastEventDate,
    int TotalEvents
);

public record NewVsRecurrentDto(
    string EventName,
    int NewUsers,
    int RecurrentUsers
);
