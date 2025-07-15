using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Factulty.Queries.GetAllFaculties;

public class GetAllFacultiesQueryHandler : IRequestHandler<GetAllFacultiesQuery, ActionResult<Response<IEnumerable<Faculty>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllFacultiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<Faculty>>>> Handle(GetAllFacultiesQuery request, CancellationToken cancellationToken)
    {
        var facultiesSearched = await _unitOfWork.FacultyService.GetAllFacultyAsync();

        return new OkObjectResult(new Response<IEnumerable<Faculty>>((int)MessageStatusCode.Success, facultiesSearched));
    }
}
