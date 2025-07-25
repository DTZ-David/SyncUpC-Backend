namespace SyncUpC.Domain.Entities.User
{
    public class StaffMember : User
    {

        public string Profession { get; set; } // E.g., "Ingeniero de Sistemas"
        public string Department { get; set; } // E.g., "Coordinación Académica"
        public string Position { get; set; }   // E.g., "Docente", "Jefe de Departamento", "Decano"
        public Faculty Faculty { get; set; }
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
            Faculty faculty,
            NotificationPreferences notificationPreferences
        ) : base(email, password, firstName, lastName, phoneNumber, UserRole.StaffMember, profilePhotoUrl, isActive, notificationPreferences)
        {
            Profession = profession;
            Department = department;
            Position = position;
            Faculty = faculty;
        }
    }
}
