using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Campuses.Queries.GetAllCampus;

internal class GetAllCampusQueryHandler : IRequestHandler<GetAllCampusQuery, ActionResult<Response<IEnumerable<Campus>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCampusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<Campus>>>> Handle(GetAllCampusQuery request, CancellationToken cancellationToken)
    {
        var careersSearched = await _unitOfWork.CampusService.GetCampuses();

        return new OkObjectResult(new Response<IEnumerable<Campus>>((int)MessageStatusCode.Success, careersSearched));
    }
}