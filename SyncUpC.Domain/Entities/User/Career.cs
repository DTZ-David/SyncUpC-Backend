using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.User
{
    public class Career : BaseEntity<string>
    {
        public Career(string name, string faculty)
        {

            Name = name;
            FacultyId = faculty;
        }


        public string Name { get; set; }
        public string FacultyId { get; set; }
    }
}
