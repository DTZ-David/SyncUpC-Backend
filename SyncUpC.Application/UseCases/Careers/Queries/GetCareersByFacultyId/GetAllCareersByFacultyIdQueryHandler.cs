using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Careers.Queries.GetCareersByFacultyId;

public class GetAllCareersByFacultyIdQueryHandler : IRequestHandler<GetAllCareersByFacultyIdQuery, ActionResult<Response<IEnumerable<Career>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCareersByFacultyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<Career>>>> Handle(GetAllCareersByFacultyIdQuery request, CancellationToken cancellationToken)
    {
        var careersSearched = await _unitOfWork.CareerService.GetByFaculty(request.facultyId);

        return new OkObjectResult(new Response<IEnumerable<Career>>((int)MessageStatusCode.Success, careersSearched));
    }
}
