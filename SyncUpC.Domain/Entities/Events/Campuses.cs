using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Events
{
    public class Campus : BaseEntity<string>
    {
        public Campus(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
