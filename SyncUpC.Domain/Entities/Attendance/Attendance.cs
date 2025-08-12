using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Attendance;

public class Attendance : BaseEntity<string>
{
    public Attendance(string eventId, List<UserAttendance> userAttendances)
    {
        EventId = eventId;
        UserAttendances = userAttendances;
    }

    public string EventId { get; set; }
    public List<UserAttendance> UserAttendances { get; set; }

}

public class UserAttendance
{
    public UserAttendance(string userId, string checkInTime, string? checkOutTime)
    {
        UserId = userId;
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
    }

    public string UserId { get; set; }
    public string CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
}