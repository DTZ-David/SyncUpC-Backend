using SyncUpC.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Ports.Services;

public interface IStudentService
{
    Task<Student> CreateStudentAsync(Student user);
    Task<Student> GetStudentByEmail (string email);
}
