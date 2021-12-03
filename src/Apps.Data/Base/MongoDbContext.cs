using MongoDB.Driver;

namespace Apps.Data.Base
{
    public class MongoDbContext
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public MongoDbContext(string url)
        {
            var urlBuilder = new MongoUrlBuilder(url);
            Client = new MongoClient(urlBuilder.ToMongoUrl());
            Database = Client.GetDatabase(urlBuilder.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return Database.GetCollection<T>(typeof(T).Name);
        }
    }
}
