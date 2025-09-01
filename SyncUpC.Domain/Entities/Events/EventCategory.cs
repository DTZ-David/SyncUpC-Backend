using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Events
{
    public class EventCategory : BaseEntity<string>
    {
        public EventCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
