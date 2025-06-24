using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Domain.Common.Helpers;

public static class ObjectIdValidation
{
    public static bool IsValidObjectId(string objectId)
    {
        return ObjectId.TryParse(objectId, out _);
    }
}
