using SyncUpC.Domain.Entities.Attendance;

namespace SyncUpC.Domain.Ports.Services
{
    public interface IAttendanceService
    {
        Task<Attendance> SubmitAnAttendance(UserAttendance attendance, string eventId);
    }
}
