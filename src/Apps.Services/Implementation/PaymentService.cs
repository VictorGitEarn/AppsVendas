using Apps.Domain.Business;
using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using MassTransit;

namespace Apps.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        public async Task ConsumeMessage(Payment message)
        {
            Console.WriteLine($"Msg com o cartão {message.CreditCard}");
        }
    }
}
