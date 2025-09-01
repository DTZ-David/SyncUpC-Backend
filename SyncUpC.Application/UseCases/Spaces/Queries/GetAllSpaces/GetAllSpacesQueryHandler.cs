using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Spaces.Queries.GetAllSpaces;

public class GetAllSpacesQueryHandler : IRequestHandler<GetAllSpacesQuery, ActionResult<Response<IEnumerable<Space>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSpacesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<Space>>>> Handle(GetAllSpacesQuery request, CancellationToken cancellationToken)
    {
        var careersSearched = await _unitOfWork.SpaceService.GetSpaces();

        return new OkObjectResult(new Response<IEnumerable<Space>>((int)MessageStatusCode.Success, careersSearched));
    }
}
