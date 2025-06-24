using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       ) : base(email, password, firstName, lastName, phoneNumber, "student", profilePhotoUrl, isActive, notificationPreferences)
        {
            Career = career;
        }
    }
}
