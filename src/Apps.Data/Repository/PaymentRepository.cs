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
    public class PaymentRepository : MongoDbRepository<Payment>
    {
        public PaymentRepository(MongoDbContext dbContext) : base(dbContext) { }

        public async Task UpdateStatus(Payment payment)
        {
            var find = Builders<Payment>.Filter.Eq(t => t._id, payment._id);
            
            var update = Builders<Payment>.Update.Set(t => t.Status, payment.Status);

            await UpdateOneAsync(find, update);
        }
    }
}
