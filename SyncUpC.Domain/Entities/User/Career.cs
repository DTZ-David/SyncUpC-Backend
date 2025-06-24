using SyncUpC.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.User
{
    public class Career 
    {
        public Career(string id, string name, Faculty faculty)
        {
            Id = id;
            Name = name;
            Faculty = faculty;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Faculty Faculty { get; set; }
    }
}
