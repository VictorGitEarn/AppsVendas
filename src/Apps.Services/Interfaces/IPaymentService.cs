using Apps.Domain.Business;
using Apps.MessageQueue.Message;

namespace Apps.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreditCard> GetCreditCard(string id);

        Task<List<CreditCard>> GetAllCards();

        Task<CreditCard> ProcessCreditCard(CreditCard creditCard);

        Task ConsumeMessage(PaymentMessage message);
    }
}
