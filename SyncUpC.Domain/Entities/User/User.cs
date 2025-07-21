using MongoDB.Bson.Serialization.Attributes;
using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.User;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Student), typeof(StaffMember))]
public class User : BaseEntity<string>
{
    public User(
        string email,
        string password,
        string name,
        string lastName,
        string phoneNumber,
        UserRole role,
        string profilePicture,
        bool isActive,
        NotificationPreferences notificationPreferences
    )
    {
        Email = email;
        Password = password;
        Name = name;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Role = role;
        ProfilePicture = profilePicture;
        IsActive = isActive;
        NotificationPreferences = notificationPreferences;
        FavoriteEventIds = new List<string>();
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public string ProfilePicture { get; set; }
    public bool IsActive { get; set; }
    public NotificationPreferences NotificationPreferences { get; set; }
    public List<string> FavoriteEventIds { get; set; } = new();
}
