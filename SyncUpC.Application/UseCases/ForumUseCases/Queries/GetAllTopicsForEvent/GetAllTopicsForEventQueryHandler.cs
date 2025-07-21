using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllTopicsForEvent
{
    public class GetAllTopicsForEventQueryHandler : IRequestHandler<GetAllTopicsForEventQuery, ActionResult<Response<IEnumerable<Forum>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTopicsForEventQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<IEnumerable<Forum>>>> Handle(GetAllTopicsForEventQuery request, CancellationToken cancellationToken)
        {

            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            var facultiesSearched = await _unitOfWork.ForumService.GetTopics(request.eventId);

            var resultDto = _mapper.Map<IEnumerable<Forum>>(facultiesSearched);

            return new OkObjectResult(new Response<IEnumerable<Forum>>((int)MessageStatusCode.Success, resultDto));
        }
    }
}
