namespace SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;

public record GetAttendanceRecordDto
    (
        string CreationDate,
        string EventId,
        List<UserAttendanceDto> UserAttendanceDto
    );

public record UserAttendanceDto(
        string Name,
        string LastName,
        string Email,
        string PhoneNumber,
        string? CheckInTime,
        string? CareerName,    // Nuevo campo
        string? FacultyName    // Nuevo campo
    );