using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyncUpC.Application.UseCases.Events.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Ports;

namespace SyncUpC.Application.UseCases.Emails.Commands.SendEmailsForEvent
{
    public class SendEmailsForEventCommandHandler : IRequestHandler<SendEmailsForEventCommand, ActionResult<Response<AcademicEventDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SendEmailsForEventCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult<Response<AcademicEventDto>>> Handle(SendEmailsForEventCommand request, CancellationToken cancellationToken)
        {
            // 1. Organizador autenticado
            var userClaim = await _unitOfWork.ClaimsService.GetUserClaim();
            var organizer = await _unitOfWork.UserService.GetUserById(userClaim.UserId)
                ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

            // 2. Buscar evento
            var eventEntity = await _unitOfWork.EventService.GetEventById(request.eventId)
                ?? throw new BusinessException("Evento no encontrado", (int)MessageStatusCode.NotFound);

            var registration = await _unitOfWork.RegistrationService.GetEmailsRegistered(request.eventId);

            if (!registration.Any())
                throw new BusinessException("No hay inscritos en este evento", (int)MessageStatusCode.BadRequest);

            // 4. Preparar correo
            var subject = $"Información del evento: {eventEntity.EventTitle}";
            var body = $@"
                        Hola,

                        Estás registrado en el evento ""{eventEntity.EventTitle}"".
                        Fecha: {eventEntity.StartDate:dd/MM/yyyy} - {eventEntity.EndDate:dd/MM/yyyy}
                        Lugar: {eventEntity.Campus.Name + eventEntity.Space.Name}

                        Recuerda asistir y estar pendiente de nuevas actualizaciones. 
                        También puedes mantenerte informado en nuestra aplicación móvil **SyncUpC**, 
                        donde encontrarás notificaciones, detalles del evento y mucho más.

                        Saludos,
                        {organizer.Name} (Organizador)
                    ";


            // 5. Enviar correo masivo
            await _unitOfWork.EmailService.SendBulkEmailAsync(registration, subject, body);

            return new OkObjectResult(new Response<string>((int)MessageStatusCode.Success, "Correos enviados correctamente"));

        }


    }
}
