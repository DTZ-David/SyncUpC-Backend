using AutoMapper;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Application.UseCases.User.Students.Dtos;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Entities.User;

namespace SyncUpC.Application;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Faculty, FacultyDto>().ReverseMap();
        CreateMap<Career, CareerDto>().ReverseMap();
        CreateMap<AcademicEvent, AcademicEventDto>().ReverseMap();
    }
}
