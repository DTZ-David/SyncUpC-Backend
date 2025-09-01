using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Registration;

namespace SyncUpC.Application.UseCases.RegistrationUseCases.Commands.RegisterEvent;

public record RegisterEventCommand(string eventId) : IRequest<ActionResult<Response<Registration>>>;
