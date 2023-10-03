using URLShorteningService.Models;

namespace URLShorteningService.BL
{
    public interface IUrlBl
    {
        Task<Url> GenerateShortUrl(Uri urlPath);
        Task<Url> GetUrl(string shortUrl);
    }
}