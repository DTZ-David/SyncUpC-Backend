using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.User
{
    public class StaffMember : User
    {
        
        public string Profession { get; set; } // E.g., "Ingeniero de Sistemas"
        public string Department { get; set; } // E.g., "Coordinación Académica"
        public string Position { get; set; }   // E.g., "Docente", "Jefe de Departamento", "Decano"

        public StaffMember(
            string email,
            string password,
            string firstName,
            string lastName,
            string phoneNumber,
            string profilePhotoUrl,
            string profession,
            string department,
            string position,
            bool isActive,
            NotificationPreferences notificationPreferences
        ) : base(email, password, firstName, lastName, phoneNumber, "staff", profilePhotoUrl, isActive, notificationPreferences)
        {
            Profession = profession;
            Department = department;
            Position = position;
        }
    }
}
