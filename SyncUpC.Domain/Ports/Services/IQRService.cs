namespace SyncUpC.Domain.Ports.Services;

public interface IQRService
{
    public byte[] GenerateQrImageAsBytes(string content);
}
