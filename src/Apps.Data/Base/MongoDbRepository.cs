using MongoDB.Bson;
using MongoDB.Driver;

namespace Apps.Data.Base
{
    public abstract class MongoDbRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        protected FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;

        public MongoDbRepository(MongoDbContext dbContext)
        {
            _collection = dbContext.GetCollection<T>();
        }

        public async Task Insert(T document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task InsertMany(List<T> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public async Task UpdateOneAsync(FilterDefinition<T> builders, UpdateDefinition<T> update)
        {
            await _collection.UpdateOneAsync(Builders<T>.Filter.And(builders), update);
        }

        public async Task<T> FindById(ObjectId id)
        {
            var filter = Filter.Eq("_id", id);

            return (await _collection.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<List<T>> FindByIds(List<ObjectId> ids)
        {
            var filter = Filter.In("_id", ids);

            return (await _collection.FindAsync(filter)).ToList();
        }

        public DeleteResult Delete(ObjectId id)
        {
            var where = Filter.And(
                Filter.Eq("_id", id)
            );

            return _collection.DeleteOne(where);
        }

        public async Task<List<T>> FindAsync(FilterDefinition<T> builders)
        {
            return (await _collection.FindAsync(builders)).ToList();
        }

        public async Task<List<T>> FindAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
        
        public void Replace(T document)
        {
            var id = document.GetType().GetProperty("_id").GetValue(document, null);

            if (!(id is ObjectId) || (ObjectId)id == default(ObjectId))
            {
                throw new ArgumentException("_id deve ser informado no objeto");
            }

            _collection.ReplaceOne(Filter.Eq("_id", id), document);
        }
    }
}
