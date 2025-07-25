using SyncUpC.Domain.Entities.Base;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Domain.Entities.Events;

public class AcademicEvent : BaseEntity<string>
{
    public AcademicEvent(Organizer organizer, string eventTitle, string eventObjective, DateTime startDate, DateTime endDate, DateTime registrationStart, DateTime registrationEnd, List<Career> careers, string eventLocation, bool targetTeachers, bool targetStudents, bool targetAdministrative, bool targetGeneral, string address, bool isVirtual, string? meetingUrl, int maxCapacity, bool requiresRegistration, bool isPublic, string status, List<string> tags, EventStats stats, string additionalDetails, List<string> imageUrls, List<string> participantProfilePictures)
    {
        Organizer = organizer;
        EventTitle = eventTitle;
        EventObjective = eventObjective;
        StartDate = startDate;
        EndDate = endDate;
        RegistrationStart = registrationStart;
        RegistrationEnd = registrationEnd;
        Careers = careers;
        EventLocation = eventLocation;
        TargetTeachers = targetTeachers;
        TargetStudents = targetStudents;
        TargetAdministrative = targetAdministrative;
        TargetGeneral = targetGeneral;
        Address = address;
        IsVirtual = isVirtual;
        MeetingUrl = meetingUrl;
        MaxCapacity = maxCapacity;
        RequiresRegistration = requiresRegistration;
        IsPublic = isPublic;
        Status = status;
        Tags = tags;
        Stats = stats;
        AdditionalDetails = additionalDetails;
        ImageUrls = imageUrls;
        ParticipantProfilePictures = participantProfilePictures;
    }

    public Organizer Organizer { get; set; }
    public string EventTitle { get; set; }
    public string EventObjective { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime RegistrationStart { get; set; }
    public DateTime RegistrationEnd { get; set; }
    public Faculty? Faculty { get; set; }
    public List<Career> Careers { get; set; } = new();
    public string EventLocation { get; set; }
    public bool TargetTeachers { get; set; }
    public bool TargetStudents { get; set; }
    public bool TargetAdministrative { get; set; }
    public bool TargetGeneral { get; set; }
    public string Address { get; set; }
    public bool IsVirtual { get; set; }
    public string? MeetingUrl { get; set; }
    public int MaxCapacity { get; set; }
    public bool RequiresRegistration { get; set; }
    public bool IsPublic { get; set; }
    public string Status { get; set; }
    public List<string> Tags { get; set; } = new();
    public EventStats Stats { get; set; }
    public string AdditionalDetails { get; set; } = null!; // speakers, agenda, etc.
    public List<string> ImageUrls { get; set; } = new(); // JPG/PNG advertising materials
    public List<string> ParticipantProfilePictures { get; set; } = new();

}
