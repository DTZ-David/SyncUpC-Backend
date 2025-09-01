using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.Events
{
    public class Space : BaseEntity<string>
    {
        public Space(string name, string description, string campusId)
        {
            Name = name;
            Description = description;
            CampusId = campusId;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CampusId { get; set; }

    }
}
