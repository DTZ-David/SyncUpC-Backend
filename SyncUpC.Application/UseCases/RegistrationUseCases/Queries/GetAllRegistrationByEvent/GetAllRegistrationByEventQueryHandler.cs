using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Registration;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.RegistrationUseCases.Queries.GetAllRegistrationByEvent;

public class GetAllRegistrationByEventQueryHandler : IRequestHandler<GetAllRegistrationByEventQuery, ActionResult<Response<Registration>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRegistrationByEventQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<Registration>>> Handle(GetAllRegistrationByEventQuery request, CancellationToken cancellationToken)
    {
        var attendance = await _unitOfWork.RegistrationService.GetRegistrationOfEvent(request.eventId);

        return new OkObjectResult(
            new Response<Registration>(
                (int)MessageStatusCode.Success,
                attendance
            )
        );
    }
}