using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.EventCategories.Queries.GetAllCategories;

internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ActionResult<Response<IEnumerable<EventCategory>>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActionResult<Response<IEnumerable<EventCategory>>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var careersSearched = await _unitOfWork.EventCategoryService.GetAllCategories();

        return new OkObjectResult(new Response<IEnumerable<EventCategory>>((int)MessageStatusCode.Success, careersSearched));
    }
}
