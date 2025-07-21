using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Commands.AddTopic;

public class AddTopicCommandHandler : IRequestHandler<AddTopicCommand, ActionResult<Response<Forum>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddTopicCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActionResult<Response<Forum>>> Handle(AddTopicCommand request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        var topic = new Forum(request.eventId, user.Id, request.title, request.content);

        await _unitOfWork.ForumService.AddTopic(topic);

        return new CreatedResult(string.Empty, new Response<Forum>((int)MessageStatusCode.Create, topic));
    }
}
