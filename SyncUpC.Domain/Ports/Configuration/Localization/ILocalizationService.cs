

namespace SyncUpC.Domain.Ports.Configuration.Localization;

public interface ILocalizationService
{
    string GetLocalizedByKey(string key);
}
