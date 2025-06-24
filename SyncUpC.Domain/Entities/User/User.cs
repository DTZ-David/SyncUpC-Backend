using MongoDB.Bson.Serialization.Attributes;
using SyncUpC.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.User;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Student), typeof(StaffMember))]
public class User : BaseEntity<string>
{
    public User(string email, string password, string name, string lastName, string phoneNumber, string role, string profilePicture, bool isActive, NotificationPreferences notificationPreferences)
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
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; } // "estudiante", "docente", "admin"
    public string ProfilePicture { get; set; }
    public bool IsActive { get; set; }
    public NotificationPreferences NotificationPreferences { get; set; }
}
