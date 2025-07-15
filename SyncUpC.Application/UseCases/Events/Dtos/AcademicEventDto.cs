using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Application.UseCases.Events.Dtos;
public record AcademicEventDto
(
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
    List<string>? ParticipantProfilePictures
);
