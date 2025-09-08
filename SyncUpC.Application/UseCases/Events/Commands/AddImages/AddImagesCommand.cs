using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Commands.AddImages;

public record AddImagesCommand(
    string EventId,
    List<string> ImageUrls,
    string? Description = null
) : IRequest<ActionResult<Response<EventImagesDto>>>;