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
        private readonly IDistributedCache _cache;
        private readonly IProductService _productService;

        public CartController(INotes notes, IUser userApp, 
                              IDistributedCache cache, IProductService productService) : base(notes, userApp)
        {
            _cache = cache;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return CustomResponse(await GetCartFromCache());
        }

        [HttpPost("{productId}/{qtty}")]
        public async Task<ActionResult> Post(string productId, int qtty)
        {
            var cart = await GetCartFromCache();

            var product = await _productService.FindById(productId);

            if (cart != default)
            {
                cart.AddProduct(product, qtty);

                await SetCartToCache(cart);
            }
            else
            {
                cart = new CartViewModels(product, qtty);

                await SetCartToCache(cart);
            }

            return CustomResponse(cart);
        }

        private string GetRecordId()
        {
            return $"Cart_{UserId}_{DateTime.Now:ddMMyyyy}";
        }

        private async Task<CartViewModels> GetCartFromCache()
        {
            var recordId = GetRecordId();

            var result = await _cache.GetRecordAsync<CartViewModels>(recordId);

            return result;
        }

        private async Task SetCartToCache(CartViewModels cart)
        {
            var recordId = GetRecordId();

            await _cache.SetRecordAsync(recordId, cart);
        }
    }
}
