using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetForumById
{
    public class GetForumByIdQueryHandler : IRequestHandler<GetForumByIdQuery, ActionResult<Response<ForumDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetForumByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActionResult<Response<ForumDto>>> Handle(GetForumByIdQuery request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            var topics = await _unitOfWork.ForumService.GetForum(request.forumId);

            var resultDto = new ForumDto(
                Id: topics.Id,
                EventId: topics.EventId,
                AuthorName: (_unitOfWork.UserService.GetUserById(topics.AuthorId)?.Result is var userAs && user != null)
                ? $"{user.Name} {user.LastName}"
                : "Desconocido",
                AuthorId: topics.AuthorId!,
                AuthorProfilePicture: _unitOfWork.UserService.GetUserById(topics.AuthorId)?.Result?.ProfilePicture ?? "",

                Title: topics.Title,
                Content: topics.Content,
                Comments: topics.Comments.Select(c => new CommentDto(
                    ForumId: c.ForumId,
                    AuthorId: c.AuthorId,
                    AuthorName: _unitOfWork.UserService.GetUserById(c.AuthorId)?.Result?.Name ?? "Desconocido",
                    AuthorProfilePicture: _unitOfWork.UserService.GetUserById(c.AuthorId)?.Result?.ProfilePicture ?? "",
                    Content: c.Content,
                    Time: c.CreationDate.ToLocalTime().ToString("g")
                )).ToList(),
                Time: topics.CreationDate.ToLocalTime().ToString("g")
            );
            return new OkObjectResult(new Response<ForumDto>((int)MessageStatusCode.Success, resultDto));

        }
    }
}
