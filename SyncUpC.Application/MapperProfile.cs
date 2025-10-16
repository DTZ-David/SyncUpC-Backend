using AutoMapper;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Application.UseCases.Faculties.Dtos;
using SyncUpC.Application.UseCases.User.Dtos;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Application;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Mapeos básicos para entidades simples
        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Career, CareerDto>().ReverseMap();
        CreateMap<Faculty, FacultiesDto>().ReverseMap();
        CreateMap<EventImages, EventImagesDto>().ReverseMap();
        // Mapeos para DTOs de records - solo de entidad a DTO
        CreateMap<Campus, CampusDto>()
            .ConstructUsing(src => new CampusDto(src.Name ?? string.Empty));

        CreateMap<Space, SpaceDto>()
            .ConstructUsing(src => new SpaceDto(src.Name ?? string.Empty));

        CreateMap<EventCategory, EventCategoryDto>()
            .ConstructUsing(src => new EventCategoryDto(src.Name ?? string.Empty));

        CreateMap<EventType, EventTypeDto>()
            .ConstructUsing(src => new EventTypeDto(src.Name ?? string.Empty));

        // Mapeo principal del evento académico
        CreateMap<AcademicEvent, AcademicEventDto>()
            .ConstructUsing(src => new AcademicEventDto(
                src.Id ?? string.Empty,
                src.EventTitle ?? string.Empty,
                src.EventObjective ?? string.Empty,
                src.StartDate,
                src.EndDate,
                // Mapeo directo y seguro
                src.Campus != null ? new CampusDto(src.Campus.Name ?? string.Empty) : new CampusDto(string.Empty),
                src.Space != null ? new SpaceDto(src.Space.Name ?? string.Empty) : new SpaceDto(string.Empty),
                src.TargetTeachers,
                src.TargetStudents,
                src.TargetAdministrative,
                src.TargetGeneral,
                src.AdditionalDetails ?? string.Empty,
                src.ImageUrls ?? new List<string>(),
                src.ParticipantProfilePictures ?? new List<string>(),
                // Mapeo seguro de colecciones
                src.Categories != null
                    ? src.Categories.Select(c => new EventCategoryDto(c.Name ?? string.Empty)).ToList()
                    : new List<EventCategoryDto>(),
                src.EventTypes != null
                    ? src.EventTypes.Select(et => new EventTypeDto(et.Name ?? string.Empty)).ToList()
                    : new List<EventTypeDto>(),
                 src.MaxCapacity,
                 src.MeetingUrl!,
                src.RequiresRegistration,
               
                false,
                false,
                src.Status ?? string.Empty
            ));
    }
}