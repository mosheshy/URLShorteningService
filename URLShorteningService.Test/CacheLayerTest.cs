

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using URLShorteningService.CacheLayer;

namespace URLShorteningService.Test
{
    [TestClass]
    public class CacheLayerTest
    {
        private readonly URLShorteningServiceTestSources _uRLShorteningServiceTestSources;
        public CacheLayerTest()
        {
            _uRLShorteningServiceTestSources = new URLShorteningServiceTestSources();
        }
        [TestMethod]
        public void AddCacheTest()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<ICacheURLShortening>();
            int cacheSize = 10;
            var inMemorySettings = new Dictionary<string, string>
            {
                {"cache_size", cacheSize.ToString()},
                {"ttlChche", "0:0:10:0" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
                .Build();

            ICacheURLShortening cacheURLShortening = new CacheURLShortening(logger, configuration);
            var url = "https://learn.microsoft.com/en-us/aspnet/core/getting-started/?view=aspnetcore-7.0&tabs=windows";
            var uri = new Uri(url);
            var shortUrl = _uRLShorteningServiceTestSources.GenerateShortUrlString(url);
            var b = cacheURLShortening.Add(shortUrl, uri);
            var cache = cacheURLShortening.GetAllCache();

            for (int i = 0; i < cacheSize + 10; i++)
            {
                url = _uRLShorteningServiceTestSources.GenerateUrlString();
                uri = new Uri(url);
                shortUrl = _uRLShorteningServiceTestSources.GenerateShortUrlString(url);
                b = cacheURLShortening.Add(shortUrl, uri);
                cache = cacheURLShortening.GetAllCache();
            }
            cache = cacheURLShortening.GetAllCache();
            Assert.IsTrue(cache.Count == cacheSize);
        }

        [TestMethod]
        public async Task CacheTimeOutTestAsync()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<ICacheURLShortening>();
            int cacheSize = 100;
            var inMemorySettings = new Dictionary<string, string>
            {
                {"cache_size", cacheSize.ToString()},
                {"ttlChche", "0:0:0:30" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
                .Build();

            ICacheURLShortening cacheURLShortening = new CacheURLShortening(logger, configuration);
            for (int i = 0; i < cacheSize; i++)
            {
                var url = _uRLShorteningServiceTestSources.GenerateUrlString();
                var uri = new Uri(url);
                var shortUrl = _uRLShorteningServiceTestSources.GenerateShortUrlString(url);
                var b = cacheURLShortening.Add(shortUrl, uri);

            }
            var cache = cacheURLShortening.GetAllCache();
            Assert.IsTrue(cache.Count == cacheSize);
            var value = cacheURLShortening.Get(cache.First().Key);
            Assert.IsNotNull(value.Value);
            await Task.Delay(35000);
            value = cacheURLShortening.Get(cache.First().Key);
            Assert.IsNull(value.Value);
        }
    }
}