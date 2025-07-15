using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.User
{
    public class Faculty : BaseEntity<string>
    {
        public Faculty(string name)
        {

            Name = name;
        }


        public string Name { get; set; }

    }
}
