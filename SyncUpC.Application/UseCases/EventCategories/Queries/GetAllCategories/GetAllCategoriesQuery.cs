using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Events;

namespace SyncUpC.Application.UseCases.EventCategories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<ActionResult<Response<IEnumerable<EventCategory>>>>;
