namespace SyncUpC.Application.UseCases.Events.Dtos;
public record AcademicEventDto
(
    string Id,
    // Event info
    string EventTitle,
    string EventObjective,
    DateTime EventDate,
    string EventLocation,

    // Target audience
    bool TargetTeachers,
    bool TargetStudents,
    bool TargetAdministrative,
    bool TargetGeneral,

    // Optional additional data
    string? AdditionalDetails,
    List<string>? ImageUrls,

    // ✅ Solo URLs de fotos de perfil de los asistentes
    List<string>? ParticipantProfilePictures,
    List<string>? Tags,
    bool isSaved
);
