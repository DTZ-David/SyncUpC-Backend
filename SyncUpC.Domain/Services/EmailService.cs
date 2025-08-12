using SendGrid;
using SendGrid.Helpers.Mail;
using SyncUpC.Domain.Ports.Services;
using SyncUpC.Domain.Services;

[ApplicationService]
public class EmailService : IEmailService
{
    private readonly string _sendGridApiKey = "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";

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("dtzdavid@outlook.com", "SyncUpC"); // o tu correo/nombre deseado
        var toEmail = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, body, body);

        // Adjuntar archivo (QR)
        string base64Content = Convert.ToBase64String(attachmentBytes);
        msg.AddAttachment(attachmentName, base64Content);

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            throw new Exception($"Error al enviar correo: {response.StatusCode}\n{responseBody}");
        }
    }
}
