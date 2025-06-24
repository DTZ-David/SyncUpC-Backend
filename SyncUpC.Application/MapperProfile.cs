using AutoMapper;
using SyncUpC.Application.UseCases.User.Students.Dtos;
using SyncUpC.Domain.Entities.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SyncUpC.Application;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Faculty, FacultyDto>().ReverseMap();
        CreateMap<Career, CareerDto>().ReverseMap();
    }
}
