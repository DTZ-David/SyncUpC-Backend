using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.EventTypes.Queries.GetAllEventTypes;

public class GetAllEventTypesQueryHandler : IRequestHandler<GetAllEventTypesQuery, ActionResult<Response<IEnumerable<EventType>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEventTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<EventType>>>> Handle(GetAllEventTypesQuery request, CancellationToken cancellationToken)
    {
        var careersSearched = await _unitOfWork.EventTypeService.GetAllEventTypes();

        return new OkObjectResult(new Response<IEnumerable<EventType>>((int)MessageStatusCode.Success, careersSearched));
    }
}
