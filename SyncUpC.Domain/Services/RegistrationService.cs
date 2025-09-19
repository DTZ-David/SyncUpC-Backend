using SyncUpC.Domain.Entities.Registration;
using SyncUpC.Domain.Entities.User;
using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Services;

namespace SyncUpC.Domain.Services;

[ApplicationService]
public class RegistrationService : IRegistrationService
{
    private readonly IGenericRepository<Registration> _attendanceRepository;
    private readonly IGenericRepository<User> _userReporistoy;

    public RegistrationService(IGenericRepository<Registration> attendanceRepository, IGenericRepository<User> userReporistoy)
    {
        _attendanceRepository = attendanceRepository;
        _userReporistoy = userReporistoy;
    }

    public async Task<bool> DeleteRegistration(string eventId, string userId)
    {
        try
        {

            var registrationList = await _attendanceRepository.FindAsync(r => r.EventId == eventId);
            var registration = registrationList.FirstOrDefault();

            if (registration == null)
            {

                return false;
            }

            var userRegistration = registration.RegistratedUsers
                .FirstOrDefault(ur => ur.UserId == userId);

            if (userRegistration == null)
            {
                // El usuario no está registrado en este evento
                return false;
            }

            // Remover el usuario de la lista de registrados
            registration.RegistratedUsers.Remove(userRegistration);

            await _attendanceRepository.Update(registration);


            return true;
        }
        catch (Exception)
        {
            // Log the exception if you have logging configured
            // _logger.LogError(ex, "Error deleting registration for eventId: {EventId}, userId: {UserId}", eventId, userId);
            return false;
        }
    }

    public async Task<List<Registration>> GetAllRegistration()
    {
        var registration = await _attendanceRepository.GetAll();
        return registration.ToList();
    }

    public async Task<List<string>> GetEmailsRegistered(string eventId)
    {
        // Buscar el registro del evento
        var registration = await _attendanceRepository.FindAsync(r => r.EventId == eventId);

        if (registration == null || !registration.Any())
            return new List<string>();

        // Extraer todos los userIds
        var userIds = registration
            .SelectMany(r => r.RegistratedUsers)
            .Select(u => u.UserId)
            .Distinct()
            .ToList();

        if (!userIds.Any())
            return new List<string>();

        // Consultar usuarios por sus IDs
        var users = await _userReporistoy.FindAsync(u => userIds.Contains(u.Id));

        // Sacar los emails
        var emails = users
            .Where(u => !string.IsNullOrEmpty(u.Email))
            .Select(u => u.Email)
            .ToList();

        return emails;
    }


    public async Task<Registration> GetRegistrationOfEvent(string eventId)
    {
        var registration = await _attendanceRepository.GetById(eventId);
        return registration;
    }

    public async Task<Registration> RegistrationEvent(UserRegistration registration, string eventId)
    {
        // Buscar si ya existe un Attendance para ese evento
        var attendanceList = await _attendanceRepository.FindAsync(a => a.EventId == eventId);

        var attendance = attendanceList.FirstOrDefault();

        if (attendance == null)
        {
            // No existe, creamos uno nuevo
            var newAttendance = new Registration(
                eventId: eventId,
                registratedUsers: new List<UserRegistration> { registration }
            );

            await _attendanceRepository.Add(newAttendance);
            return newAttendance;
        }
        else
        {
            // Ya existe el Attendance, actualizamos o agregamos el UserAttendance
            var existingUserAttendance = attendance.RegistratedUsers
                .FirstOrDefault(ua => ua.UserId == registration.UserId);

            if (existingUserAttendance != null)
            {
                // Actualizar los campos necesarios
                existingUserAttendance.RegistrationDate = registration.RegistrationDate;

            }
            else
            {
                // Agregar nuevo UserAttendance
                attendance.RegistratedUsers.Add(registration);
            }

            await _attendanceRepository.Update(attendance);

            return attendance;
        }
    }
}
