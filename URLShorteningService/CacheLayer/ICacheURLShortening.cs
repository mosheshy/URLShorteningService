using System.Collections.Concurrent;
using URLShorteningService.Models;

namespace URLShorteningService.CacheLayer
{
    public interface ICacheURLShortening
    {
        bool Add(string key, Uri value);
        void ClenCache();
        KeyValuePair<string, Cache> Get(string key);
        ConcurrentDictionary<string, Cache> GetAllCache();
        bool RemoveCache(string key);
    }
}