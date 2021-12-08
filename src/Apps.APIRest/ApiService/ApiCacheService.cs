using Apps.APIRest.Extentions;
using Apps.APIRest.Models.ViewModels;
using Apps.Domain.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;

namespace Apps.APIRest.ApiService
{
    public class ApiCacheService
    {
        private readonly IDistributedCache _cache;

        public ApiCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        private static string GetRecordId(ObjectId userId) => $"Cart_{userId}_{DateTime.Now:ddMMyyyy}";

        public async Task<CartViewModels> AddProduct(Product product, int qtty, ObjectId userId)
        {
            var cart = await GetCartFromCache(userId);

            if (cart != default)
            {
                cart.AddProduct(product, qtty);

                await SetCartToCache(cart, userId);
            }
            else
            {
                cart = new CartViewModels(product, qtty);

                await SetCartToCache(cart, userId);
            }

            return cart;
        }

        public async Task<CartViewModels> GetCart(ObjectId userId) => await GetCartFromCache(userId);

        public async Task RemoveProduct(Product product, int qtty, ObjectId userId)
        {
            var cart = await GetCartFromCache(userId);

            if (cart == default)
                return;

            cart.RemoveProduct(product, qtty);

            await RefreshCart(cart, userId);
        }

        private async Task<CartViewModels> GetCartFromCache(ObjectId userId)
        {
            var recordId = GetRecordId(userId);

            var result = await _cache.GetRecordAsync<CartViewModels>(recordId);

            return result;
        }

        private async Task SetCartToCache(CartViewModels cart, ObjectId userId)
        {
            var recordId = GetRecordId(userId);

            await _cache.SetRecordAsync(recordId, cart);
        }

        public async Task DeleteCart(ObjectId userId)
        {
            var recordId = GetRecordId(userId);

            await _cache.DeleteRecordAsync(recordId);
        }

        private async Task RefreshCart(CartViewModels cart, ObjectId userId)
        {
            await DeleteCart(userId);

            await SetCartToCache(cart, userId);
        }
    }
}
