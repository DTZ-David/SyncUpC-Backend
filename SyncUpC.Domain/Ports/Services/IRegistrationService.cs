using SyncUpC.Domain.Entities.Registration;

namespace SyncUpC.Domain.Ports.Services
{
    public interface IRegistrationService
    {
        Task<Registration> RegistrationEvent(UserRegistration registration, string eventId);
        Task<List<Registration>> GetAllRegistration();
        Task<Registration> GetRegistrationOfEvent(string eventId);
        Task<List<string>> GetEmailsRegistered(string eventId);

    }
}
