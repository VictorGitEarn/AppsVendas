using Apps.Data.Base;
using Apps.Domain.Business;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Data.Repository
{
    public class PurchaseRepository : MongoDbRepository<Purchase>
    {
        public PurchaseRepository(MongoDbContext dbContext) : base(dbContext) { }

        public async Task<Purchase> FindByPaymentId(List<ObjectId> paymentId)
        {
            var filter = Builders<Purchase>.Filter;

            var where = filter.And(
                filter.In("Payments", paymentId)
            );

            return (await FindAsync(where)).FirstOrDefault();
        }

        public async Task UpdateStatus(Purchase purchase)
        {
            var find = Builders<Purchase>.Filter.Eq(t => t._id, purchase._id);

            var update = Builders<Purchase>.Update.Set(t => t.Status, purchase.Status);

            await UpdateOneAsync(find, update);
        }

        public async Task<List<Purchase>> GetAllByUser(ObjectId userId)
        {
            var filter = Builders<Purchase>.Filter;

            var where = filter.And(
                filter.Eq(x => x.UserId, userId)
            );

            return await FindAsync(where);

        }

        public async Task<Purchase> GetById(ObjectId id,ObjectId userId)
        {
            var filter = Builders<Purchase>.Filter;

            var where = filter.And(
                filter.Eq(x => x._id, id),
                filter.Eq(x => x.UserId, userId)
            );

            return (await FindAsync(where)).FirstOrDefault();
        }
    }
}
