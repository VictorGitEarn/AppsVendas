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
    public class CreditCardRepository : MongoDbRepository<CreditCard>
    {
        public CreditCardRepository(MongoDbContext dbContext) : base(dbContext) { }

        public async Task<CreditCard> FindCreditCardByNumber(string cardNumber)
        {
            var filter = Builders<CreditCard>.Filter;

            var where = filter.And(
                filter.Eq(x => x.CreditCardNumber, cardNumber)
            );

            return (await FindAsync(where)).FirstOrDefault();
        }

        public async Task<List<CreditCard>> FindAllByUser(ObjectId userId)
        {
            var filter = Builders<CreditCard>.Filter;

            var where = filter.And(
                filter.Eq(x => x.UserId, userId)
            );

            return await FindAsync(where);
        }
    }
}
