using SyncUpC.Domain.Entities.Base;

namespace SyncUpC.Domain.Entities.User;

public class Faculty : BaseEntity<string>
{
    public Faculty(string name, List<Career> careers)
    {
        Name = name;
        Careers = careers;
    }
    public Faculty()
    {

    }

    public string Name { get; set; }
    public List<Career> Careers { get; set; }
}
