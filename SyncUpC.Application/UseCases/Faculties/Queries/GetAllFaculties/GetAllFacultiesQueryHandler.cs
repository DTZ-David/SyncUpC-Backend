using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Faculties.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Factulties.Queries.GetAllFaculties;

public class GetAllFacultiesQueryHandler : IRequestHandler<GetAllFacultiesQuery, ActionResult<Response<IEnumerable<FacultiesDto>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllFacultiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<FacultiesDto>>>> Handle(GetAllFacultiesQuery request, CancellationToken cancellationToken)
    {
        var facultiesSearched = await _unitOfWork.FacultyService.GetAllFacultyAsync();

        var resultDto = _mapper.Map<IEnumerable<FacultiesDto>>(facultiesSearched);

        return new OkObjectResult(new Response<IEnumerable<FacultiesDto>>((int)MessageStatusCode.Success, resultDto));
    }
}
