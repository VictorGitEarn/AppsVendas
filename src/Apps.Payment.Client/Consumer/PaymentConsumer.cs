using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Apps.Payment.Client.Consumer
{
    public class PaymentConsumer : IConsumer<PaymentMessage>
    {
        private readonly IPaymentService _paymentService;

        public PaymentConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public void Attach(IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("apps_payments", e =>
            {
                e.Lazy = true;

                e.PrefetchCount = 20;
                
                e.Bind<PaymentMessage>();

                e.Consumer(() => this);
            });
        }

        public async Task Consume(ConsumeContext<PaymentMessage> context)
        {
            var message = context.Message;

            await _paymentService.ConsumeMessage(new Domain.Business.Payment() { CreditCard = message.CreditCard });
        }
    }
}
