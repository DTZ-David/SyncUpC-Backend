using SendGrid;
using SendGrid.Helpers.Mail;
using SyncUpC.Domain.Ports.Services;
using SyncUpC.Domain.Services;

[ApplicationService]
public class EmailService : IEmailService
{

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
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("dtzdavid@outlook.com", "SyncUpC");
        var toEmail = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, body, body);

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            throw new Exception($"Error al enviar correo: {response.StatusCode}\n{responseBody}");
        }
    }

    public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("dtzdavid@outlook.com", "SyncUpC");

        var toEmails = recipients.Select(r => new EmailAddress(r)).ToList();

        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmails, subject, body, body);

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            throw new Exception($"Error al enviar correos masivos: {response.StatusCode}\n{responseBody}");
        }
    }

}
