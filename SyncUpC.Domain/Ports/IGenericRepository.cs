using SyncUpC.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Ports;

public interface IGenericRepository<E> where E : BaseEntity<string>
{
    Task<IEnumerable<E>> GetAll();
    Task<E> GetById(string id);
    Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> filter);
    Task Add(E entity);
    Task Update(E entity);
    Task Delete(E entity);
    Task<bool> Exist(string id);
}
