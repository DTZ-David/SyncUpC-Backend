using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddComment
{
    internal class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, ActionResult<Response<Forum>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<Forum>>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);


            var forum = await _unitOfWork.ForumService.GetForum(request.forumId)
                ?? throw new BusinessException("El foro no existe", (int)MessageStatusCode.NotFound);


            var comment = new Comment(request.forumId, user.Id, request.content);

            forum.Comments.Add(comment);

            await _unitOfWork.ForumService.AddComment(request.forumId, comment);

            return new OkObjectResult(new Response<Forum>((int)MessageStatusCode.Success, forum));

        }
    }
}
