using MongoDB.Bson;
using MongoDB.Driver;

namespace Apps.Data.Base
{
    public abstract class MongoDbRepository<T> : IMongoDbRepository<T>
    {
        private readonly IMongoCollection<T> _collection;
        protected FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;

        public MongoDbRepository(MongoDbContext dbContext)
        {
            _collection = dbContext.GetCollection<T>();
        }

        public void Insert(T document)
        {
            _collection.InsertOne(document);
        }

        public void Replace(T document)
        {
            var id = document.GetType().GetProperty("_id").GetValue(document, null);

            if (id is not ObjectId || (ObjectId)id == default)
            {
                throw new ArgumentException("_id deve ser informado no objeto");
            }

            _collection.ReplaceOne(Filter.Eq("_id", id), document);
        }

        public T FindById(ObjectId id)
        {
            var filter = Filter.Eq("_id", id);

            return _collection.FindSync(filter).FirstOrDefault();
        }

        public async Task<List<T>> FindById(ObjectId[] ids)
        {
            var filter = Filter.In("_id", ids);

            return (await _collection.FindAsync(filter)).ToList();
        }

        public DeleteResult Delete(ObjectId id)
        {
            var where = Filter.Eq("_id", id);

            return _collection.DeleteOne(where);
        }
    }
}
