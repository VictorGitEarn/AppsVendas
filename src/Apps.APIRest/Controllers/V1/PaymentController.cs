using Apps.Domain.Business;
using Apps.Domain.Business.Interfaces;
using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PaymentController : MainController
    {
        private readonly IPaymentService _paymentService;
        private readonly IBusControl _bus;

        public PaymentController(INotes notes, IUser userApp, IPaymentService paymentService, IBusControl bus) : base(notes, userApp)
        {
            _paymentService = paymentService;
            _bus = bus;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            await _bus.StartAsync();

            try
            {
                var enpoint = await _bus.GetSendEndpoint(new Uri("exchange:apps_payments"));

                await enpoint.Send(new PaymentMessage() { CreditCard = "testes testes" });
            }
            catch (Exception ex)
            {
                NotifyError("Impossibilitado de processar o pagamento, favor entrar em contato com o suporte.");
                return CustomResponse();
            }
            finally
            {
                await _bus.StopAsync();
            }

            return CustomResponse();
        }
    }
}
