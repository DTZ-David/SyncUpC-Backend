using System.Net;
using System.Net.Mail;
using SyncUpC.Domain.Ports.Services;
using SyncUpC.Domain.Services;

[ApplicationService]
public class EmailService : IEmailService
{
  

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName)
    {
        using var message = new MailMessage(_gmailUser, to, subject, body)
        {
            IsBodyHtml = true
        };

        // Adjuntar archivo
        using var stream = new MemoryStream(attachmentBytes);
        message.Attachments.Add(new Attachment(stream, attachmentName));

        await SendEmailAsync(message);
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var message = new MailMessage(_gmailUser, to, subject, body)
        {
            IsBodyHtml = true
        };

        await SendEmailAsync(message);
    }

    public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_gmailUser),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        foreach (var recipient in recipients)
        {
            message.Bcc.Add(recipient); // Usa BCC para emails masivos
        }

        await SendEmailAsync(message);
    }

    private async Task SendEmailAsync(MailMessage message)
    {
        using var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_gmailUser, _gmailAppPassword)
        };

        try
        {
            await client.SendMailAsync(message);
        }
        catch (SmtpException ex)
        {
            throw new Exception($"Error al enviar correo: {ex.Message}", ex);
        }
    }
}