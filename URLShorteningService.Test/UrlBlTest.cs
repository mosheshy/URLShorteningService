using Amazon.Runtime.Internal.Transform;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using URLShorteningService.BL;
using URLShorteningService.Data;

namespace URLShorteningService.Test
{
    [TestClass]
    public class UrlBlTest
    {
        private readonly URLShorteningServiceTestSources _uRLShorteningServiceTestSources;
        public UrlBlTest()
        {
            _uRLShorteningServiceTestSources = new URLShorteningServiceTestSources();
        }

        [TestMethod]
        public async Task GenerateShortUrlTest()
        {
            int ShortUrlLength = 8;
            var inMemorySettings = new Dictionary<string, string>
            {
                {"shortUrlLength", ShortUrlLength.ToString()},
                {"numOfPrevent", "250" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
                .Build();
            IUrlsRepository urlsRepository = new UrlsRepository(new MongoClient("mongodb+srv://moshe:gD8QGr_TyAd.DwT@cluster0.n7ivmhe.mongodb.net/?retryWrites=true&w=majority"));
            IUrlBl urlBl = new UrlBl(urlsRepository,configuration);
            var url = _uRLShorteningServiceTestSources.GenerateUrlString();
            var uri = new Uri(url);
            var model= await  urlBl.GenerateShortUrl(uri);
            Assert.IsTrue(model != null);
            Assert.IsTrue(model._id.Length == ShortUrlLength);
            urlsRepository.Remove(model._id);


        }
        [TestMethod]
        public async Task GetUrlTest()
        {
            int ShortUrlLength = 8;
            var inMemorySettings = new Dictionary<string, string>
            {
                {"shortUrlLength", ShortUrlLength.ToString()},
                {"numOfPrevent", "250" }
                
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
                .Build();
            IUrlsRepository urlsRepository = new UrlsRepository(new MongoClient("mongodb+srv://moshe:gD8QGr_TyAd.DwT@cluster0.n7ivmhe.mongodb.net/?retryWrites=true&w=majority"));
            IUrlBl urlBl = new UrlBl(urlsRepository, configuration);
            var url = _uRLShorteningServiceTestSources.GenerateUrlString();
            var uri = new Uri(url);
            var model = await urlBl.GenerateShortUrl(uri);
            Assert.IsTrue(model != null);
            var data = await urlBl.GetUrl(model._id);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Uri == uri);


            urlsRepository.Remove(model._id);


        }
        [TestMethod]
        [Ignore]
        public async Task GenerateDataBaseMokDataTest()
        {
            int ShortUrlLength = 8;
            var inMemorySettings = new Dictionary<string, string>
            {
                {"shortUrlLength", ShortUrlLength.ToString()},
                {"numOfPrevent", "250" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(inMemorySettings)
                .Build();
            IUrlsRepository urlsRepository = new UrlsRepository(new MongoClient("mongodb+srv://moshe:gD8QGr_TyAd.DwT@cluster0.n7ivmhe.mongodb.net/?retryWrites=true&w=majority"));
            IUrlBl urlBl = new UrlBl(urlsRepository, configuration);

            for (int i = 0; i < 400; i++)
            {

                var url = _uRLShorteningServiceTestSources.GenerateUrlString();
                var uri = new Uri(url);
                var model = await urlBl.GenerateShortUrl(uri);
                Assert.IsTrue(model != null);
                Assert.IsTrue(model._id.Length == ShortUrlLength);

            }

        }




    }
}
