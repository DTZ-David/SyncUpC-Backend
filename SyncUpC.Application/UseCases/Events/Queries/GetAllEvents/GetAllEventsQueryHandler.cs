using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Events.Queries.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, ActionResult<Response<IEnumerable<AcademicEventDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventService.GetAllEvents();

            var resultDto = _mapper.Map<IEnumerable<AcademicEventDto>>(events);

            return new OkObjectResult(new Response<IEnumerable<AcademicEventDto>>((int)MessageStatusCode.Success, resultDto));
        }
    }
}
