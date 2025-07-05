using ECommerceAIMockUp.Application.Services.Interfaces.Caching;
using ECommerceAIMockUp.Application.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace ECommerceAIMockUp.Infrastructure.Services.Caching
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache? _cache;
        private readonly RedisSettings _redisSettings;

        public RedisService(IDistributedCache? cache, IOptions<RedisSettings> RedisSettings)
        {
            _cache = cache;
            _redisSettings = RedisSettings.Value;
        }

        public T? GetData<T>(string Key)
        {
            var data = _cache.GetString(Key);

            if (data is null)
                return default(T);

            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T Data)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes((long)_redisSettings.Time)
            };

            _cache.SetString(key, JsonSerializer.Serialize(Data), options);
        }
    }
}
