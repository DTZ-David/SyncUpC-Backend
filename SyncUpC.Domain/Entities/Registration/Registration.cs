using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Registration;

public class Registration : BaseEntity<string>
{
    public Registration(string eventId, List<UserRegistration> registratedUsers)
    {
        EventId = eventId;
        RegistratedUsers = registratedUsers;
    }

    public string EventId { get; set; }
    public List<UserRegistration> RegistratedUsers { get; set; }


}

public class UserRegistration
{
    public UserRegistration(string userId, DateTime registrationDate)
    {
        UserId = userId;
        RegistrationDate = registrationDate;
    }

    public string UserId { get; set; }
    public DateTime RegistrationDate { get; set; }
}
