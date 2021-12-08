using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business;
using Apps.Domain.Business.Interfaces;
using Apps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PurchaseController : MainController
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;

        public PurchaseController(INotes notes, IUser userApp,
                                  IPurchaseService purchaseService, IPaymentService paymentService, IProductService productService) : base(notes, userApp)
        {
            _purchaseService = purchaseService;
            _paymentService = paymentService;
            _productService = productService;
        }

        [HttpPost("close-purchase")]
        public async Task<ActionResult> Post(string purchaseId, List<PaymentModel> payments)
        {
            var purchase = await _purchaseService.Get(new ObjectId(purchaseId), UserId);

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (purchase is null)
            {
                NotifyError("Não foi localizado o pedido informado.");
                return CustomResponse();
            }

            if (purchase.Status is not PurchaseStatus.Open)
            {
                NotifyError("Não é possível fechar pedido com status diferente de Aberto.");
                return CustomResponse();
            }

            if (ValidateValueFromPurchase_Payments(purchase, payments) is false)
            {
                NotifyError($"Valor do pedido e pagamentos não batem, favor verificar, Pedido {purchase.Value} x Pagamentos {payments.Sum(t => t.Value)}.");
                return CustomResponse();
            }

            return CustomResponse(await _purchaseService.ClosePurchase(purchase, await GetPaymentsFromModel(payments)));
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetAll()
        {
            var purchases = await _purchaseService.GetAll(UserId);

            var result = new List<PurchaseModel>();

            foreach (var purchase in purchases)
            {
                result.Add(await MapToModel(purchase));
            }

            return CustomResponse(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string id) => CustomResponse(await MapToModel(await _purchaseService.Get(new ObjectId(id), UserId)));

        [HttpDelete]
        public async Task<ActionResult> DeleteById(string id)
        {
            if (!await _purchaseService.DeletePurchase(new ObjectId(id), UserId))
            {
                NotifyError($"Não foi localizado perdido com o id {id} ou o Pedido não está com Status Aberto");
            }

            return CustomResponse();
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
                var payment = model.MapToPayment(UserId);

                var creditCard = await _paymentService.ProcessCreditCard(model.CreditCard.MapToCreditCard(UserId));

                payment.CreditCardId = creditCard._id;

                result.Add(payment);
            }

            return result;
        }

        private async Task<PurchaseModel> MapToModel(Purchase purchase)
        {
            var products = await _productService.FindByIds(purchase.Products.Select(t => t.ProductId).ToList());

            var payments = new List<Payment>();
            
            if (purchase.Payments is not null)
                payments = await _paymentService.FindByIds(purchase.Payments);

            var creditCards = new List<CreditCard>();
            
            if (payments.Any())
                creditCards = await _paymentService.FindCardsByIds(payments.Select(t => t.CreditCardId).ToList());

            return new PurchaseModel(purchase, products, payments, creditCards);
        }
    }
}
