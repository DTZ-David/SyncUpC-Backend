using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Context;

public class MongoContext<T>
{
    private readonly IMongoDatabase _database;
    public MongoContext(IMongoDatabase database)
    {
        _database = database;
    }
    public IMongoCollection<T> DataBase => _database.GetCollection<T>(typeof(T).Name);
}
