using Apps.APIRest.ApiService;
using Apps.APIRest.Extentions;
using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business.Interfaces;
using Apps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Apps.APIRest.Controllers.V1
{
    [ApiVersion("1.0")]
    public class CartController : MainController
    {
        private readonly IProductService _productService;
        private readonly IPurchaseService _purchaseService;
        private readonly ApiCacheService _apiCacheService;

        public CartController(INotes notes, IUser userApp, IDistributedCache cache, 
                              IProductService productService, IPurchaseService purchaseService) : base(notes, userApp)
        {
            _productService = productService;
            _apiCacheService = new ApiCacheService(cache);
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<ActionResult> Get() => CustomResponse(await _apiCacheService.GetCart(UserId));

        [HttpPost("{productId}/{qtty}")]
        public async Task<ActionResult> Post(string productId, int qtty)
        {
            var product = await _productService.FindById(productId);

            return CustomResponse(await _apiCacheService.AddProduct(product, qtty, UserId));
        }

        [HttpDelete("{productId}/{qtty}")]
        public async Task<ActionResult> RemoveProduct(string productId, int qtty)
        {
            var product = await _productService.FindById(productId);

            await _apiCacheService.RemoveProduct(product, qtty, UserId);

            return CustomResponse();
        }

        [HttpPost("close-cart")]
        public async Task<ActionResult> CloseCart()
        {
            var cart = await _apiCacheService.GetCart(UserId);

            if (cart == default)
            {
                NotifyError("Não é possívle fechar carrinho pois o mesmo não existe, tente adicionar um itempara criar um carrinho.");
                return CustomResponse();
            }

            var purchase = cart.MapToPurchase();

            await _apiCacheService.DeleteCart(UserId);

            return CustomResponse(await _purchaseService.Create(purchase));
        }
    }
}
