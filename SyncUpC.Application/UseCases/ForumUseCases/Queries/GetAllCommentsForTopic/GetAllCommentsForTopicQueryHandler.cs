using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Forum;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllCommentsForTopic;

public class GetAllCommentsForTopicQueryHandler : IRequestHandler<GetAllCommentsForTopicQuery, ActionResult<Response<IEnumerable<Comment>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCommentsForTopicQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<Comment>>>> Handle(GetAllCommentsForTopicQuery request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        var facultiesSearched = await _unitOfWork.ForumService.GetAllCommentForTopic(request.forumId);

        var resultDto = _mapper.Map<IEnumerable<Comment>>(facultiesSearched);

        return new OkObjectResult(new Response<IEnumerable<Comment>>((int)MessageStatusCode.Success, resultDto));
    }
}
