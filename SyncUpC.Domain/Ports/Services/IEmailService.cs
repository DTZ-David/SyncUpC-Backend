namespace SyncUpC.Domain.Ports.Services;

public interface IEmailService
{
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName);
    Task SendEmailAsync(string to, string subject, string body);
    Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body);

}
