using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.ForumUseCases.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.ForumUseCases.Queries.GetAllCommentsForTopic;

public class GetAllCommentsForTopicQueryHandler : IRequestHandler<GetAllCommentsForTopicQuery, ActionResult<Response<IEnumerable<CommentDto>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCommentsForTopicQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<CommentDto>>>> Handle(GetAllCommentsForTopicQuery request, CancellationToken cancellationToken)
    {
        var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        var comments = await _unitOfWork.ForumService.GetAllCommentForTopic(request.forumId);

        var resultDto = comments.Select(c => new CommentDto(
                    ForumId: c.ForumId,
                    AuthorId: c.AuthorId,
                    AuthorName: _unitOfWork.UserService.GetUserById(c.AuthorId)?.Result?.Name ?? "Desconocido",
                    AuthorProfilePicture: _unitOfWork.UserService.GetUserById(c.AuthorId)?.Result?.ProfilePicture ?? "",
                    Content: c.Content,
                    Time: c.CreationDate.ToLocalTime().ToString("g")
                )).ToList();

        return new OkObjectResult(new Response<IEnumerable<CommentDto>>((int)MessageStatusCode.Success, resultDto));
    }
}
