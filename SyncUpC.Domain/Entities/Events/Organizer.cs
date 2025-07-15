namespace SyncUpC.Domain.Entities.Events;

public class Organizer
{
    public Organizer(string id, string institutionalEmail, string phoneNumber, string name)
    {
        Id = id;
        InstitutionalEmail = institutionalEmail;
        PhoneNumber = phoneNumber;
        Name = name;
    }

    public string Id { get; set; }
    public string InstitutionalEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
}
