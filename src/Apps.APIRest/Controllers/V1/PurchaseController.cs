using Apps.APIRest.ApiService;
using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business;
using Apps.Domain.Business.Interfaces;
using Apps.MessageQueue.Message;
using Apps.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PurchaseController : MainController
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IPaymentService _paymentService;

        public PurchaseController(INotes notes, IUser userApp, 
                                  IPurchaseService purchaseService, IPaymentService paymentService) : base(notes, userApp)
        {
            _purchaseService = purchaseService;
            _paymentService = paymentService;
        }

        [HttpPost("close-purchase")]
        public async Task<ActionResult> Post(string purchaseId, List<PaymentModel> payments)
        {
            var purchase = await _purchaseService.Get(purchaseId);

            if (ValidateValueFromPurchase_Payments(purchase, payments))
            {
                NotifyError($"Valor do pedido e pagamentos não batem, favor verificar, Pedido {purchase.Value} x Pagamentos {payments.Sum(t => t.Value)}.");
                return CustomResponse();
            }

            return CustomResponse(_purchaseService.ClosePurchase(purchase, await GetPaymentsFromModel(payments)));
        }

        private static bool ValidateValueFromPurchase_Payments(Purchase purchase, List<PaymentModel> paymentModel)
        {
            return purchase.Value == paymentModel.Sum(t => t.Value);
        }

        private async Task<List<Payment>> GetPaymentsFromModel(List<PaymentModel> payments)
        {
            var result = new List<Payment>();

            foreach (var model in payments)
            {
                var payment = model.MapToPayment();

                var creditCard = await _paymentService.ProcessCreditCard(model.CreditCard.MapToCreditCard());

                payment.CreditCardId = creditCard._id;

                result.Add(payment);
            }

            return result;
        }
    }
}
