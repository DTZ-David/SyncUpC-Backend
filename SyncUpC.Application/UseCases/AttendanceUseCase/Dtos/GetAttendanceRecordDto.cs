namespace SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;

public record GetAttendanceRecordDto
    (
        string CreationDate,
        string EventId,
        List<UserAttendanceDto> UserAttendanceDto
    );

public record UserAttendanceDto(
    string Nombre,
    string Apellido,
    string Numero,
    string CheckInTime
    );