namespace SyncUpC.Application.UseCases.Events.Dtos;
// ================== SubDtos ==================
public record CampusDto(
    string Name
);

public record SpaceDto(

    string Name

);

public record EventCategoryDto(
    string Name
);

public record EventTypeDto(

    string Name

);

// ================== Evento principal ==================
public record AcademicEventDto
(
    string Id,
    // Event info
    string EventTitle,
    string EventObjective,
    DateTime EventStartDate,
    DateTime EventEndDate,

    // Ubicación
    CampusDto Campus,
    SpaceDto Space,

    // Públicos objetivos
    bool TargetTeachers,
    bool TargetStudents,
    bool TargetAdministrative,
    bool TargetGeneral,

    // Extras
    string? AdditionalDetails,
    List<string>? ImageUrls,
    List<string>? ParticipantProfilePictures,

    // Clasificación
    List<EventCategoryDto> Categories,
    List<EventTypeDto> EventTypes,
    int MaxCapacity,
    string MeetingUrl,
    bool? RequiresRegistration,
    bool? IsSaved,
    bool? IsRegistered,
    string Status
);