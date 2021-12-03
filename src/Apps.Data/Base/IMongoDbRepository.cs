using MongoDB.Bson;
using MongoDB.Driver;

namespace Apps.Data.Base
{
    public interface IMongoDbRepository<T>
    {
        void Insert(T document);

        void Replace(T document);
        
        T FindById(ObjectId id);
        
        Task<List<T>> FindById(ObjectId[] ids);
        
        DeleteResult Delete(ObjectId id);
    }
}
