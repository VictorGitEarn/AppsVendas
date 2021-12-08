using Apps.Domain.Business;
using MongoDB.Bson;

namespace Apps.Services.Interfaces
{
    public interface IPurchaseService
    {
        public Task<Purchase> Get(ObjectId id, ObjectId userId);

        public Task<List<Purchase>> GetAll(ObjectId userId);

        public Task<bool> DeletePurchase(ObjectId id, ObjectId userId);

        public Task<Purchase> Create(Purchase purchase, ObjectId userId);

        public Task<bool> ClosePurchase(Purchase purchase, List<Payment> payment);
    }
}
