using SyncUpC.Domain.Entities.Attendance;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;



namespace SyncUpC.Domain.Services;

[ApplicationService]
public class AttendanceService : IAttendanceService
{
    private readonly IGenericRepository<Attendance> _attendanceRepository;

    public AttendanceService(IGenericRepository<Attendance> attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public async Task<Attendance> SubmitAnAttendance(UserAttendance newUserAttendance, string eventId)
    {
        // Buscar si ya existe un Attendance para ese evento
        var attendanceList = await _attendanceRepository.FindAsync(a => a.EventId == eventId);

        var attendance = attendanceList.FirstOrDefault();

        if (attendance == null)
        {
            // No existe, creamos uno nuevo
            var newAttendance = new Attendance(
                eventId: eventId,
                userAttendances: new List<UserAttendance> { newUserAttendance }
            );

            await _attendanceRepository.Add(newAttendance);
            return newAttendance;
        }
        else
        {
            // Ya existe el Attendance, actualizamos o agregamos el UserAttendance
            var existingUserAttendance = attendance.UserAttendances
                .FirstOrDefault(ua => ua.UserId == newUserAttendance.UserId);

            if (existingUserAttendance != null)
            {
                // Actualizar los campos necesarios
                existingUserAttendance.CheckInTime = newUserAttendance.CheckInTime;
                existingUserAttendance.CheckOutTime = newUserAttendance.CheckOutTime;
            }
            else
            {
                // Agregar nuevo UserAttendance
                attendance.UserAttendances.Add(newUserAttendance);
            }

            await _attendanceRepository.Update(attendance);

            return attendance;
        }
    }

}
