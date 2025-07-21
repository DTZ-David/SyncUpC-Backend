using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.User
{
    public class Career : BaseEntity<string>
    {
        public Career(string name)
        {
            Name = name;

        }

        public string Name { get; set; }
    }
}
