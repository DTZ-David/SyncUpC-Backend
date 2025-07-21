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
        CreateMap<Student, StudentDto>().ReverseMap();

        CreateMap<Career, CareerDto>().ReverseMap();

        // Configuración manual para el record
        CreateMap<AcademicEvent, AcademicEventDto>()
            .ConstructUsing(src => new AcademicEventDto(
                src.EventTitle,
                src.EventObjective,
                src.StartDate,
                src.EventLocation,
                src.TargetTeachers,
                src.TargetStudents,
                src.TargetAdministrative,
                src.TargetGeneral,
                src.AdditionalDetails,
                src.ImageUrls,
                src.ParticipantProfilePictures
            ));



        CreateMap<Faculty, FacultiesDto>().ReverseMap();
    }
}
