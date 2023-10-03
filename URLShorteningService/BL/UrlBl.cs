using System.Security.Cryptography;
using System.Text;
using URLShorteningService.Data;
using URLShorteningService.Models;

namespace URLShorteningService.BL
{
    public class UrlBl : IUrlBl
    {
        private readonly IUrlsRepository _urlsRepository;
        private readonly int _shortUrlLength;
        private readonly int _numOfPrevent;

        public UrlBl(IUrlsRepository urlsRepository, IConfiguration configuration)
        {
            _urlsRepository = urlsRepository;
            _shortUrlLength = configuration.GetValue<int>("shortUrlLength");
            _numOfPrevent = configuration.GetValue<int>("numOfPrevent");
        }
        public async Task<Url> GenerateShortUrl( Uri urlPath)
        {
            int i = 1;
            
            var id = GenerateShortUrlString(urlPath.ToString());
            var oldUrl = await GetUrl(id);
            while(oldUrl != null) 
            { 
                
                if(oldUrl._id == id)
                {
                    if(i == _numOfPrevent)
                    {
                        throw new Exception("Unable to create short Url for this url");
                    }

                    if(oldUrl.Uri == urlPath)
                    {
                        return oldUrl;
                    }
                    else
                    {
                         id = GenerateShortUrlString(i + urlPath.ToString());
                         oldUrl = await GetUrl(id);
                         i++;
                    }
                }
            }
            var url = new Url
            {
                Uri =urlPath,
                _id = id
            };

            url = await _urlsRepository.Creat(url);
            return url;
        }
        public async Task<Url> GetUrl(string shortUrl)
        {
           var url = await  _urlsRepository.get(shortUrl);
            return url;
        }
        private string GenerateShortUrlString(string url)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {

                    byte[] bytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(url));
                    var shortrl = new string(Convert.ToBase64String(bytes, 0, bytes.Length)
                    .Where(char.IsLetterOrDigit)
                    .Take(_shortUrlLength)
                    .ToArray());

                    if (shortrl.Length == _shortUrlLength)
                    {
                        return shortrl;
                    }
                    throw new Exception($"error {shortrl.Length}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} error ");
            }
        }       
    }
}
