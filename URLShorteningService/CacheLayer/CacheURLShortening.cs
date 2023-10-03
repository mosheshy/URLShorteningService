using System.Collections.Concurrent;
using URLShorteningService.Models;

namespace URLShorteningService.CacheLayer
{
    public class CacheURLShortening : ICacheURLShortening
    {
        private readonly ConcurrentDictionary<string, Cache> _cacheDictionary;
        private readonly TimeSpan _ttlChche;
        private readonly int _capacity;
        private readonly object _lock = new object();
        private readonly ILogger<ICacheURLShortening> _logger;

        public CacheURLShortening(ILogger<ICacheURLShortening> logger, IConfiguration configuration)
        {
            logger.LogInformation("Cache is on");
            _logger = logger;
            int concurrencyLevel = Environment.ProcessorCount * 2;
            _capacity = configuration.GetValue<int>("cache_size");
            _ttlChche = configuration.GetValue<TimeSpan>("ttlChche");
            _cacheDictionary = new ConcurrentDictionary<string, Cache>(concurrencyLevel, _capacity);
        }
        public bool Add(string key, Uri value)
        {
            try
            {
                lock (_lock)
                {
                    if (_cacheDictionary.Count == _capacity)
                    {
                        var lastUsed = _cacheDictionary.Values.Min(r => r.LastUsed);
                        var oldCache = _cacheDictionary.Select(e => e).FirstOrDefault();
                        _cacheDictionary.TryRemove(oldCache);
                    }
                    var cache = new Cache
                    {
                        ExpiredTime = DateTime.UtcNow + _ttlChche,
                        Url = value,
                        LastUsed = DateTime.UtcNow,
                    };
                    _cacheDictionary.TryAdd(key, cache);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public KeyValuePair<string, Cache> Get(string key)
        {
            KeyValuePair<string, Cache> keyValuePair = new KeyValuePair<string, Cache>(key, null);
            try
            {

                lock (_lock)
                {
                    if (_cacheDictionary.TryGetValue(key, out var cached))
                    {
                        if (cached.ExpiredTime > DateTime.UtcNow)
                        {
                            var newCache = new Cache
                            {
                                Url = cached.Url,
                                ExpiredTime = cached.ExpiredTime,
                                LastUsed = DateTime.UtcNow
                            };
                            _cacheDictionary.TryUpdate(key, newCache, cached);
                            keyValuePair = new KeyValuePair<string, Cache>(key, cached);
                        }
                        else
                        {
                            var cacheItem = new KeyValuePair<string, Cache>(key, cached);
                            _cacheDictionary.TryRemove(cacheItem);                            
                        }
                    }
                }
                return keyValuePair;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return keyValuePair;
        }
        public bool RemoveCache(string key)
        {
            bool result = false;
            lock (_lock)
            {
                if (_cacheDictionary.TryGetValue(key.ToString(), out var cached))
                {
                    var cacheItem = new KeyValuePair<string, Cache>(key, cached);
                    _cacheDictionary.TryRemove(cacheItem);
                    result = true;
                }
            }
            return result;
        }
        public ConcurrentDictionary<string, Cache> GetAllCache()
        {
            return _cacheDictionary;
        }
        public void ClenCache()
        {
            _cacheDictionary.Clear();
        }
    }
}