using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace CoinCapApi
{
    internal class MemCache : IDisposable
    {
        internal bool Enabled { get; set; } = true;

        private readonly ILogger _logger;
        private readonly List<string> _keys;
        private readonly MemoryCache _cache;
        private readonly object _lockObject;

        internal MemCache(ILogger<CoinCapClient> logger)
        {
            _logger = logger;
            _cache = new MemoryCache("response-cache");
            _keys = new List<string>();
            _lockObject = new object();
        }

        private void CacheRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            lock (_lockObject)
            {
                _keys.Remove(arguments.CacheItem.Key);
            }
        }

        internal bool Contains(string key) => _cache.Contains(key);

        internal bool TryGet(string key, out object value)
        {
            if (!Enabled)
            {
                _logger?.LogDebug("Cache Disabled for URL: {Key}", key);

                value = null;
                return false;
            }

            if (_cache.Contains(key))
            {
                _logger?.LogDebug("Cache Hit for URL: {Key}", key);

                value = _cache.Get(key);
                return true;
            }
            else
            {
                _logger?.LogDebug("Cache Miss for URL: {Key}", key);
                value = null;
                return false;
            }
        }

        internal void CacheRequest(string key, RestResponse response, int cacheSeconds)
        {
            if (!Enabled) { return; }

            var data = response.Content;

            if (!string.IsNullOrEmpty(data) && !string.IsNullOrWhiteSpace(data))
            {
                if (cacheSeconds <= Constants.API_MIN_CACHE_TIME_MS / 1000) { cacheSeconds = Convert.ToInt16(Constants.API_MIN_CACHE_TIME_MS / 1000); }

                var expiry = DateTimeOffset.UtcNow.AddSeconds(cacheSeconds);

                if (expiry < DateTimeOffset.UtcNow.AddMinutes(2))
                {
                    Set(key, data, expiry);
                    _logger?.LogDebug("Cache Set Expires in: {Expiry} seconds for URL: {Key}", cacheSeconds, key);
                }
                else
                {
                    _logger?.LogWarning("The expires is too far in the future. Expiry: {Expiry}, URL: {FullUrl}", expiry, key);
                }
            }
        }

        private void Set(string key, object value, DateTimeOffset exp)
        {
            lock (_lockObject)
            {
                var cacheItem = new CacheItem(key, value);
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = exp
                };
                policy.RemovedCallback += CacheRemovedCallback;

                _cache.Set(cacheItem, policy);

                if (!_keys.Contains(key)) { _keys.Add(key); }
            }
        }

        internal void Clear()
        {
            lock (_lockObject)
            {
                var keys = _keys.ToArray();
                foreach (var key in keys)
                {
                    try
                    {
                        _cache.Remove(key);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                try
                {
                    _keys.Clear();
                }
                catch
                {
                    // ignore
                }
            }
        }

        public void Dispose() => _cache.Dispose();
    }
}
