using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Apps.APIRest.Extentions
{
    public static class CacheExtention
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, 
                                                   TimeSpan? absoluteExpireTime = null, TimeSpan? unsedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(60);
            
            options.SlidingExpiration = unsedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            
            if (jsonData is null)
                return default;

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public static async Task DeleteRecordAsync(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            
            if (jsonData is not null)
                cache.Remove(recordId);
        }
    }
}
