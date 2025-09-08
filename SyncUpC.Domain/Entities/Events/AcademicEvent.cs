using SyncUpC.Domain.Entities.Base;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Domain.Entities.Events;

public class AcademicEvent : BaseEntity<string>
{
    public AcademicEvent(
        Organizer organizer,
        string eventTitle,
        string eventObjective,
        DateTime startDate,
        DateTime endDate,
        Campus campus,
        Space space,
        List<Faculty> faculty,
        List<Career> careers,
        bool targetTeachers,
        bool targetStudents,
        bool targetAdministrative,
        bool targetGeneral,
        bool isVirtual,
        string? meetingUrl,
        int maxCapacity,
        bool requiresRegistration,
        bool isPublic,
        string status,
        List<EventCategory> categories,
        List<EventType> eventTypes,
        string additionalDetails,
        List<string> imageUrls,
        List<string> participantProfilePictures)
    {
        Organizer = organizer;
        EventTitle = eventTitle;
        EventObjective = eventObjective;
        StartDate = startDate;
        EndDate = endDate;
        Campus = campus;
        Space = space;
        Faculties = faculty;
        Careers = careers;
        TargetTeachers = targetTeachers;
        TargetStudents = targetStudents;
        TargetAdministrative = targetAdministrative;
        TargetGeneral = targetGeneral;
        IsVirtual = isVirtual;
        MeetingUrl = meetingUrl;
        MaxCapacity = maxCapacity;
        RequiresRegistration = requiresRegistration;
        IsPublic = isPublic;
        Status = status;
        Categories = categories;
        EventTypes = eventTypes;
        AdditionalDetails = additionalDetails;
        ImageUrls = imageUrls;
        ParticipantProfilePictures = participantProfilePictures;
    }

    public Organizer Organizer { get; set; }
    public string EventTitle { get; set; }
    public string EventObjective { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Ubicación
    public Campus Campus { get; set; }
    public Space Space { get; set; }

    // Públicos objetivos
    public List<Faculty> Faculties { get; set; } = new();
    public List<Career> Careers { get; set; } = new();
    public bool TargetTeachers { get; set; }
    public bool TargetStudents { get; set; }
    public bool TargetAdministrative { get; set; }
    public bool TargetGeneral { get; set; }

    // Modalidad
    public bool IsVirtual { get; set; }
    public string? MeetingUrl { get; set; }

    // Configuración
    public int MaxCapacity { get; set; }
    public bool RequiresRegistration { get; set; }
    public bool IsPublic { get; set; }
    public string Status { get; set; }

    // Clasificación
    public List<EventCategory> Categories { get; set; } = new();
    public List<EventType> EventTypes { get; set; } = new();

    // Extras
    public string? AdditionalDetails { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public List<string> ParticipantProfilePictures { get; set; } = new();

}
