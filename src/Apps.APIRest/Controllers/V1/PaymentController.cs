using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business.Interfaces;
using Apps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PaymentController : MainController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(INotes notes, IUser userApp, IPaymentService paymentService) : base(notes, userApp)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-all-cards")]
        public async Task<ActionResult> GetCards() => CustomResponse((await _paymentService.GetAllCards(UserId)).Select(t => new CreditCardModel(t)));

        [HttpGet("get-card")]
        public async Task<ActionResult> GetCard(string id) => CustomResponse(new CreditCardModel(await _paymentService.GetCreditCard(new ObjectId(id), UserId)));
    }
}
