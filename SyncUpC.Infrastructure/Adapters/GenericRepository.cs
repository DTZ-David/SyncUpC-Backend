using MongoDB.Driver;
using SyncUpC.Domain.Entities.Base;
using SyncUpC.Domain.Ports;
using System.Linq.Expressions;

namespace SyncUpC.Infraestructure.Adapters;


public class GenericRepository<E> : IGenericRepository<E> where E : BaseEntity<string>
{
    private readonly IMongoCollection<E> _collection;

    public GenericRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<E>(typeof(E).Name);
    }

    public async Task<IEnumerable<E>> GetAll()
    {
        return await _collection.Find(e => e.Id != null).ToListAsync();
    }

    public async Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<E> GetById(string id)
    {
        var filter = Builders<E>.Filter.And(
            Builders<E>.Filter.Eq(e => e.Id, id)
        );
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task Add(E entity)
    {
        entity.SetCreationDate();
        await _collection.InsertOneAsync(entity);
    }

    public async Task Update(E entity)
    {
        var filter = Builders<E>.Filter.Eq(e => e.Id, entity.Id);

        entity.SetModificationDate();
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task Delete(E entity)
    {

        var filter = Builders<E>.Filter.And(
            Builders<E>.Filter.Eq(e => e.Id, entity.Id)
        );
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<bool> Exist(string id)
    {
        var filter = Builders<E>.Filter.Eq("_id", id);
        var result = await _collection.Find(filter).FirstOrDefaultAsync();
        return result != null;
    }

}
