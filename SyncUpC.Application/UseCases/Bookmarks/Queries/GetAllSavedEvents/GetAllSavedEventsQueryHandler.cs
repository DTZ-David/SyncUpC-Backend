using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Bookmarks.Queries.GetAllSavedEvents
{
    public class GetAllSavedEventsQueryHandler : IRequestHandler<GetAllSavedEventsQuery, ActionResult<Response<IEnumerable<AcademicEventDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSavedEventsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<IEnumerable<AcademicEventDto>>>> Handle(GetAllSavedEventsQuery request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("Usuario no encontrado", (int)MessageStatusCode.NotFound);

            var academicEvent = await _unitOfWork.EventService.GetSavedEvents(user.FavoriteEventIds)
                ?? throw new BusinessException("Evento no encontrado", (int)MessageStatusCode.NotFound);

            var resultDto = _mapper.Map<IEnumerable<AcademicEventDto>>(academicEvent);

            return new OkObjectResult(new Response<IEnumerable<AcademicEventDto>>((int)MessageStatusCode.Success, resultDto));
        }
    }
}
