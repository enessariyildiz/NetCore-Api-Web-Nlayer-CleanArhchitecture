﻿using App.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace App.Caching
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        public Task AddAsync<T>(string cacheKey, T value, TimeSpan exprTimeSpan)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exprTimeSpan,
            };

            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string cacheKey)
        {
            return Task.FromResult(memoryCache.TryGetValue(cacheKey, out T cacheItem) ? cacheItem : default(T));
        }

        public Task RemoveAsync(string cacheKey)
        {
            memoryCache.Remove(cacheKey);

            return Task.CompletedTask;
        }
    }
}
