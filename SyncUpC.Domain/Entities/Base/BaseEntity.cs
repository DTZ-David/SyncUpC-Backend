using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Entities.Base;


[BsonIgnoreExtraElements]
public class BaseEntity<T> : BaseAuditableEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual T Id { get; set; } = default!;
}
