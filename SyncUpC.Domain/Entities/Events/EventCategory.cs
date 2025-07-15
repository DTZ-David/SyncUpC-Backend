namespace SyncUpC.Domain.Entities.Events;

public class EventCategory
{
    public EventCategory(string id, string title, string color)
    {
        Id = id;
        Title = title;
        Color = color;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
}
