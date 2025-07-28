using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddTopic;

public class AddTopicCommandHandler : IRequestHandler<AddTopicCommand, ActionResult<Response<ForumDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddTopicCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<ForumDto>>> Handle(AddTopicCommand request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();

        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        var topic = new Forum(request.eventId, user.Id, request.title, request.content);

        await _unitOfWork.ForumService.AddTopic(topic);

        // Obtener datos del autor
        var topicAuthor = await _unitOfWork.UserService.GetUserById(topic.AuthorId);

        // Como es nuevo, no debería tener comentarios, pero igual dejamos preparado
        var commentDtos = new List<CommentDto>();

        var resultDto = new ForumDto(
            topic.Id,
            topic.EventId,
            topicAuthor?.Name ?? "Usuario desconocido",
            topic.AuthorId,
            topicAuthor?.ProfilePicture ?? "",
            topic.Title,
            topic.Content,
            commentDtos,
            topic.CreationDate.ToLocalTime().ToString("g")
        );

        return new CreatedResult(string.Empty, new Response<ForumDto>((int)MessageStatusCode.Create, resultDto));
    }

}
