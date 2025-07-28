using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddComment
{
    internal class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, ActionResult<Response<ForumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<ForumDto>>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

            var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            var forum = await _unitOfWork.ForumService.GetForum(request.forumId)
                ?? throw new BusinessException("El foro no existe", (int)MessageStatusCode.NotFound);

            var comment = new Comment(request.forumId, user.Id, request.content);

            forum.Comments.Add(comment);

            await _unitOfWork.ForumService.AddComment(request.forumId, comment);

            // Obtener los datos del autor del foro
            var forumAuthor = await _unitOfWork.UserService.GetUserById(forum.AuthorId);

            // Obtener los datos de los autores de los comentarios
            var commentAuthors = new Dictionary<string, (string name, string profilePic)>();
            foreach (var c in forum.Comments)
            {
                if (!commentAuthors.ContainsKey(c.AuthorId))
                {
                    var commentAuthor = await _unitOfWork.UserService.GetUserById(c.AuthorId);
                    commentAuthors[c.AuthorId] = (
                        commentAuthor?.Name ?? "Desconocido",
                        commentAuthor?.ProfilePicture ?? ""
                    );
                }
            }

            var resultDto = new ForumDto(
                forum.Id,
                forum.EventId,
                forumAuthor?.Name ?? "Usuario desconocido",
                forum.AuthorId!,
                forumAuthor?.ProfilePicture ?? "",
                forum.Title,
                forum.Content,
                forum.Comments.Select(c => new CommentDto(
                    c.ForumId,
                    c.AuthorId,
                    commentAuthors[c.AuthorId].name,
                    commentAuthors[c.AuthorId].profilePic,
                    c.Content,
                    c.CreationDate.ToLocalTime().ToString("g")
                )).ToList(),
                forum.CreationDate.ToLocalTime().ToString("g")
            );

            return new OkObjectResult(new Response<ForumDto>((int)MessageStatusCode.Success, resultDto));
        }

    }
}
