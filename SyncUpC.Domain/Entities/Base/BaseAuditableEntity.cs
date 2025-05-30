using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.Base;

public class BaseAuditableEntity
{
    public DateTime? ModificationDate { get; set; }
    public DateTime CreationDate { get; set; }

    public void SetModificationDate() => ModificationDate = DateTime.UtcNow;
    public void SetCreationDate() => CreationDate = DateTime.UtcNow;
}
