using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SyncUpC.Application.UseCases.AttendanceUseCase.Commands.FillAttendance;
using SyncUpC.Application.UseCases.AttendanceUseCase.Dtos;
using SyncUpC.Domain.Common.Enums;
using SyncUpC.Domain.Common.Exceptions;
using SyncUpC.Domain.Common.Wrappers.CustomResponse;
using SyncUpC.Domain.Entities.Attendance;
using SyncUpC.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Application.UseCases.AttendanceUseCase.Commands.CheckInAttendanceAdmin;

internal class CheckInAttendanceAdminCommandHandler : IRequestHandler<CheckInAttendanceAdminCommand, ActionResult<Response<AttendanceDto>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CheckInAttendanceAdminCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;

        // Configuración requerida por QuestPDF
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<ActionResult<Response<AttendanceDto>>> Handle(CheckInAttendanceAdminCommand request, CancellationToken cancellationToken)
    {
        // Obtener usuario autenticado
        var claims = await _unitOfWork.ClaimsService.GetUserClaim();
        var user = await _unitOfWork.UserService.GetUserById(request.userId)
            ?? throw new BusinessException("ERROR DE AUTENTICIDAD", (int)MessageStatusCode.NotFound);

        // Consultar el evento
        var eventEntity = await _unitOfWork.EventService.GetEventById(request.eventId)
            ?? throw new BusinessException("El evento no existe", (int)MessageStatusCode.NotFound);

        var now = DateTime.UtcNow;

        if (eventEntity.EndDate < now)
        {
            throw new BusinessException("El evento ya finalizó, no es posible registrar asistencia.", (int)MessageStatusCode.BadRequest);
        }

        if (eventEntity.StartDate > now)
        {
            throw new BusinessException("El evento aún no ha iniciado, no es posible registrar asistencia.", (int)MessageStatusCode.BadRequest);
        }

        // Crear el objeto UserAttendance
        var userAttendance = new UserAttendance(
            userId: user.Id,
            checkInTime: now.ToString("o")
        );

        var attendance = await _unitOfWork.AttendanceService.SubmitAnAttendance(userAttendance, request.eventId);

        // ========== GENERAR Y ENVIAR CERTIFICADO PDF ==========
        try
        {
            var certificatePdf = GenerateCertificatePdf($"{user.Name} {user.LastName}", eventEntity.EventTitle, now);

            var subject = "Certificado de Asistencia - " + eventEntity.EventTitle;
            var body = GenerateEmailBody($"{user.Name} {user.LastName}", eventEntity.EventTitle, now);

            await _unitOfWork.EmailService.SendEmailWithAttachmentAsync(
                to: user.Email,
                subject: subject,
                body: body,
                attachmentBytes: certificatePdf,
                attachmentName: $"Certificado_Asistencia_{eventEntity.EventTitle.Replace(" ", "_")}.pdf"
            );
        }
        catch (Exception ex)
        {
            // Si falla el envío del email, solo loguear pero no detener el proceso
            Console.WriteLine($"Error al enviar certificado: {ex.Message}");
        }
        // ======================================================

        var attendanceDto = new AttendanceDto(eventEntity.Id, eventEntity.EventTitle);
        return new CreatedResult(string.Empty, new Response<AttendanceDto>((int)MessageStatusCode.Create, attendanceDto));
    }

    private byte[] GenerateCertificatePdf(string userName, string eventTitle, DateTime checkInTime)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);

                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, userName, eventTitle, checkInTime));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            // Nota: Si tienes los logos, usa: row.ConstantItem(80).Image("ruta/al/logo.png");
            row.ConstantItem(80).Height(60).Border(1).BorderColor(Colors.Green.Darken2)
                .AlignCenter().AlignMiddle().Text("UPC").FontSize(16).Bold().FontColor(Colors.Green.Darken2);

            row.RelativeItem().Column(column =>
            {
                column.Item().AlignCenter().Text("UNIVERSIDAD POPULAR DEL CESAR")
                    .FontSize(16).Bold().FontColor(Colors.Green.Darken3);
                column.Item().AlignCenter().Text("Programa de Ingeniería de Sistemas")
                    .FontSize(12).FontColor(Colors.Grey.Darken2);
                column.Item().PaddingTop(5).AlignCenter().Text("CERTIFICADO DE ASISTENCIA")
                    .FontSize(18).Bold().FontColor(Colors.Green.Darken2);
            });

            row.ConstantItem(80).Height(60).Border(1).BorderColor(Colors.Green.Darken2)
                .AlignCenter().AlignMiddle().Text("Ingeniería\nde Sistemas")
                .FontSize(10).Bold().FontColor(Colors.Green.Darken2);
        });
    }

    private void ComposeContent(IContainer container, string userName, string eventTitle, DateTime checkInTime)
    {
        container.PaddingVertical(30).Column(column =>
        {
            column.Spacing(15);

            column.Item().AlignCenter().Text($"Generado el: {checkInTime:dd/MM/yyyy} a las {checkInTime:HH:mm} (UTC)")
                .FontSize(10).FontColor(Colors.Grey.Medium);

            column.Item().PaddingTop(20).LineHorizontal(2).LineColor(Colors.Green.Darken2);

            column.Item().PaddingTop(30).Text("Se certifica que:").FontSize(14).Bold();

            column.Item().PaddingTop(10).AlignCenter()
                .Border(2).BorderColor(Colors.Green.Darken2)
                .Padding(15)
                .Text(userName.ToUpper())
                .FontSize(20).Bold().FontColor(Colors.Green.Darken3);

            column.Item().PaddingTop(30).Text("Asistió al evento:").FontSize(14).Bold();

            column.Item().PaddingTop(10).AlignCenter()
                .Background(Colors.Green.Lighten4)
                .Padding(15)
                .Text(eventTitle)
                .FontSize(16).Bold().FontColor(Colors.Green.Darken3);

            column.Item().PaddingTop(30).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Fecha:").FontSize(12).Bold();
                    col.Item().PaddingTop(5).Text(checkInTime.ToString("dddd, dd 'de' MMMM 'de' yyyy"))
                        .FontSize(12).FontColor(Colors.Grey.Darken1);
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Hora de registro:").FontSize(12).Bold();
                    col.Item().PaddingTop(5).Text(checkInTime.ToString("HH:mm:ss"))
                        .FontSize(12).FontColor(Colors.Grey.Darken1);
                });
            });

            column.Item().PaddingTop(20).LineHorizontal(2).LineColor(Colors.Green.Darken2);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.Span("Sistema de Gestión de Asistencia").FontSize(9).FontColor(Colors.Grey.Medium);
            text.Span("\nUniversidad Popular del Cesar - Programa de Ingeniería de Sistemas").FontSize(8).FontColor(Colors.Grey.Medium);
        });
    }

    private string GenerateEmailBody(string userName, string eventTitle, DateTime checkInTime)
    {
        return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                .header {{ background-color: #2d7a3e; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                .content {{ background-color: #f9f9f9; padding: 30px; border: 1px solid #ddd; }}
                .footer {{ background-color: #f1f1f1; padding: 15px; text-align: center; font-size: 12px; color: #666; border-radius: 0 0 5px 5px; }}
                .highlight {{ background-color: #e8f5e9; padding: 10px; border-left: 4px solid #2d7a3e; margin: 15px 0; }}
                .button {{ display: inline-block; padding: 12px 24px; background-color: #2d7a3e; color: white; text-decoration: none; border-radius: 5px; margin-top: 15px; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>✅ Asistencia Confirmada</h1>
                </div>
                <div class='content'>
                    <p>Hola <strong>{userName}</strong>,</p>
                    
                    <p>Tu asistencia ha sido registrada exitosamente.</p>
                    
                    <div class='highlight'>
                        <strong>📅 Evento:</strong> {eventTitle}<br>
                        <strong>🕐 Fecha y hora:</strong> {checkInTime:dd/MM/yyyy HH:mm}<br>
                    </div>
                    
                    <p>Adjunto a este correo encontrarás tu <strong>certificado de asistencia en formato PDF</strong>.</p>
                    
                    <p>Gracias por tu participación.</p>
                </div>
                <div class='footer'>
                    <p>Sistema de Gestión de Asistencia<br>
                    Universidad Popular del Cesar - Programa de Ingeniería de Sistemas</p>
                </div>
            </div>
        </body>
        </html>";
    }

   

}
