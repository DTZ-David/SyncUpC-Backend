namespace SyncUpC.Domain.Entities.User
{
    public class Student : User
    {
        public Career Career { get; set; }
        public Student(
           string email,
           string password,
           string firstName,
           string lastName,
           string phoneNumber,
           string profilePhotoUrl,
           bool isActive,
           Career career,
           NotificationPreferences notificationPreferences
       ) : base(email, password, firstName, lastName, phoneNumber, UserRole.Student, profilePhotoUrl, isActive, notificationPreferences)
        {
            Career = career;
        }
    }
}
