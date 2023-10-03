using MongoDB.Driver;
using URLShorteningService.Models;

namespace URLShorteningService.Data
{
    public class UrlsRepository : IUrlsRepository
    {
        private readonly IMongoCollection<Url> _urlDB;
        public UrlsRepository(IMongoClient mongoClient)
        {
            var dataBase = mongoClient.GetDatabase("mosheURLShorteningDB");
            _urlDB = dataBase.GetCollection<Url>("Urls");
        }
        public List<Url> GetList() => _urlDB.Find(url => url != null).ToList();
        public async Task<Url> get(string id)
        {
            var url = await _urlDB.FindAsync(url => url._id == id);
            return url.FirstOrDefault();
        }
        public async Task<Url> Creat( Url url)
        {            
            await _urlDB.InsertOneAsync(url);
            return url;
        }       
        public bool Remove(string id)
        {
            var b = _urlDB.DeleteOne(r => r._id == id);
            return b.DeletedCount > 0;
        }
    }
}
