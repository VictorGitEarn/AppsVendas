using Apps.Domain.Business;
using Apps.MessageQueue.Message;
using MongoDB.Bson;

namespace Apps.Services.Interfaces
{
    public interface IPaymentService
    {
        Task InsertPayments(List<Payment> payments);

        Task<CreditCard> GetCreditCard(ObjectId id, ObjectId userId);

        Task<List<CreditCard>> GetAllCards(ObjectId userId);

        Task<CreditCard> ProcessCreditCard(CreditCard creditCard);

        Task ConsumeMessage(PaymentMessage message);

        Task UpdatePaymentStatus(Payment payment, PaymentStatus status);

        Task<List<Payment>> FindByIds(List<ObjectId> ids);

        Task<List<CreditCard>> FindCardsByIds(List<ObjectId> ids);
    }
}
