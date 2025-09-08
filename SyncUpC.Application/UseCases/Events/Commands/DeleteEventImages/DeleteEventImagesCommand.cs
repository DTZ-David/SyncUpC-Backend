using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;

namespace SyncUpC.Application.UseCases.Events.Commands.DeleteEventImages;

public record DeleteEventImagesCommand(
    string Id
) : IRequest<ActionResult<Response<bool>>>;