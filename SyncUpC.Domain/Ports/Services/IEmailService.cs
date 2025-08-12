namespace SyncUpC.Domain.Ports.Services;

public interface IEmailService
{
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName);
}
