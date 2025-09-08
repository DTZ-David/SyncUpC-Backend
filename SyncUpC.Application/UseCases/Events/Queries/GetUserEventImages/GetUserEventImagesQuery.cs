using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Queries.GetUserEventImages;

public record GetUserEventImagesQuery : IRequest<ActionResult<Response<List<EventImagesDto>>>>;
