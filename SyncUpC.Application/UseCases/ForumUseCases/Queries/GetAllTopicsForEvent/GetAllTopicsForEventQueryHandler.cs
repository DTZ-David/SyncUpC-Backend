using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllTopicsForEvent
{
    public class GetAllTopicsForEventQueryHandler : IRequestHandler<GetAllTopicsForEventQuery, ActionResult<Response<IEnumerable<ForumDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTopicsForEventQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<IEnumerable<ForumDto>>>> Handle(GetAllTopicsForEventQuery request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            var topics = await _unitOfWork.ForumService.GetTopics(request.eventId);

            var resultDto = topics.Select(e => new ForumDto(
                e.Id,
                e.EventId,
                AuthorName: e.AuthorId != null
                    ? _unitOfWork.UserService.GetUserById(e.AuthorId)?.Result?.Name ?? "Usuario desconocido"
                    : "Usuario desconocido",
                AuthorId: e.AuthorId!,
                AuthorProfilePicture: e.AuthorId != null
                    ? _unitOfWork.UserService.GetUserById(e.AuthorId)?.Result?.ProfilePicture ?? ""
                    : "",
                Title: e.Title,
                Content: e.Content,
                Comments: e.Comments.Select(c => new CommentDto(
                    ForumId: c.ForumId,
                    AuthorId: c.AuthorId,
                    AuthorName: _unitOfWork.UserService.GetUserById(c.AuthorId)?.Result?.Name ?? "Desconocido",
                    AuthorProfilePicture: _unitOfWork.UserService.GetUserById(c.AuthorId)?.Result?.ProfilePicture ?? "",
                    Content: c.Content,
                    Time: c.CreationDate.ToLocalTime().ToString("g")
                )).ToList(),
                Time: e.CreationDate.ToLocalTime().ToString("g")
            ));

            return new OkObjectResult(new Response<IEnumerable<ForumDto>>((int)MessageStatusCode.Success, resultDto));
        }

    }
}
